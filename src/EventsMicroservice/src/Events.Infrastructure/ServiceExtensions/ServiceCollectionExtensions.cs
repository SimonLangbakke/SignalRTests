using Events.Application.Contracts.ExternalApis;
using Events.Application.Contracts.Repositories;
using Events.Infrastructure.Context;
using Events.Infrastructure.ExternalApis;
using Events.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure.ServiceExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IUserApiService, UserApiService>();
        return services;
    }

    public static IServiceCollection ConfigureDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<EventsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("FoodClubEventsDbConnection"))
        );

        return services;
    }

    public static IServiceCollection ConfigureExternalApis(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
        services.AddScoped<IPaymentService, StripeService>();
        return services;
    }

    public static void ApplyMigrations(this IServiceCollection services)
    {
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<EventsDbContext>();
        dbContext.Database.Migrate();
    }
}
