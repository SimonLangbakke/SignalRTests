using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Events.Infrastructure.Context;

public class EventsDbContextFactory : IDesignTimeDbContextFactory<EventsDbContext>
{
    public EventsDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("FoodClubEventsDbConnection")
                               ?? throw new InvalidOperationException("Connection string 'FoodClubEventsDbConnection' not found.");

        var builder = new DbContextOptionsBuilder<EventsDbContext>();
        builder.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(EventsDbContext).Assembly.FullName));

        return new EventsDbContext(builder.Options);
    }
}
