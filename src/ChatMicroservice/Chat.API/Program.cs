var builder = WebApplication.CreateBuilder(args);

// Add services following your pattern
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add memory cache for UserAPI responses
builder.Services.AddMemoryCache();

// Add HttpContextAccessor (same as your UserService uses)
builder.Services.AddHttpContextAccessor();

// Configure database (PostgreSQL like your UserAPI, but separate database)
builder.Services.ConfigureDatabase(builder.Configuration);

// Configure application services (following your pattern)
builder.Services.ConfigureApplicationServices(builder.Configuration);

// Configure infrastructure services
builder.Services.ConfigureInfrastructureServices();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<SendMessageRequestValidator>();

// Configure HttpClient for UserAPI calls
builder.Services.AddHttpClient<IUserIntegrationService, UserIntegrationService>(client =>
{
    // This would be your API Gateway URL in production
    client.BaseAddress = new Uri(builder.Configuration["Services:UserApi:BaseUrl"]);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddPolicyHandler(GetRetryPolicy()); // Add resilience

// Add SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

// Configure JWT Authentication (same as your UserAPI)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };

        // Support authentication for SignalR
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

// Apply migrations (following your pattern)
if (builder.Configuration.GetValue<bool>("AutoMigrate"))
{
    builder.Services.ApplyMigrations();
}

app.Run();