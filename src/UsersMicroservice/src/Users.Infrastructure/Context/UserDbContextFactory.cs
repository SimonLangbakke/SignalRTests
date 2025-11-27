using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Users.Infrastructure.Context;

internal class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("FoodClubUserDbConnection")
                               ?? throw new InvalidOperationException("Connection string 'FoodClubUserDbConnection' not found.");

        var builder = new DbContextOptionsBuilder<UserDbContext>();
        builder.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName));

        return new UserDbContext(builder.Options);
    }
}
