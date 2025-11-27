using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Infrastructure.EntityConfigurations;

namespace Users.Infrastructure.Context;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
    }
}