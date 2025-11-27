using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Application.Contracts.Services;
using Users.Application.Services;
using Users.Application.Settings;

namespace Users.Application.ServiceCollectionExtensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
    }
}
