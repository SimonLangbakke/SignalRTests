
using FluentValidation;
using Microsoft.OpenApi.Models;
using Users.Api.Middlewares;
using Users.Application.ServiceCollectionExtensions;
using Users.Infrastructure.ServiceExtensions;

namespace Users.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.ConfigureApplicationServices(builder.Configuration);
        builder.Services.ConfigureInfrastructureServices();
        builder.Services.ConfigureDatabase(builder.Configuration);
        builder.Services.ApplyMigrations();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Users API", Version = "v1" });
        });

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseExceptionHandler(options => { });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseCors();

        app.Run();
    }
}
