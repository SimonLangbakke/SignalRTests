using ChatMicroservice.API.Hubs;
using ChatMicroservice.API.Middlewares;
using ChatMicroservice.Application.ServiceCollectionExtensions;
using ChatMicroservice.Infrastructure.ServiceExtensions;

namespace ChatMicroservice.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure services
        builder.Services.ConfigureApplicationServices();
        builder.Services.ConfigureInfrastructureServices();
        builder.Services.ConfigureDatabase(builder.Configuration);
        builder.Services.ApplyMigrations();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();

        builder.Services.ConfigureJwtBearerAuthentication(builder.Configuration);
        builder.Services.ConfigureSwagger();
        builder.Services.AddSignalR();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API v1");
            c.RoutePrefix = string.Empty;
        });

        app.UseExceptionHandler(options => { });
        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHub<ChatHub>("/hubs/chat");

        app.Run();
    }
}