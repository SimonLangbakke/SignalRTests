using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Application.Contracts.Repositories;
using Users.Infrastructure.Context;
using Users.Infrastructure.Repository;

namespace Users.Infrastructure.ServiceExtensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("FoodClubUserDbConnection")));
    }

    public static void ApplyMigrations(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        dbContext.Database.Migrate();
    }
}
