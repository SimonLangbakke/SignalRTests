using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ChatMicroservice.Infrastructure.Context;

internal class ChatDbContextFactory : IDesignTimeDbContextFactory<ChatDbContext>
{
    public ChatDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("FoodClubChatDbConnection")
                               ?? throw new InvalidOperationException("Connection string 'FoodClubChatDbConnection' not found.");

        var builder = new DbContextOptionsBuilder<ChatDbContext>();
        builder.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(ChatDbContext).Assembly.FullName));

        return new ChatDbContext(builder.Options);
    }
}