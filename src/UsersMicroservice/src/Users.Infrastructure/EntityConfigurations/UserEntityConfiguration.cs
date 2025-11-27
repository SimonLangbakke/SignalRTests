using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.Property(u => u.Id);
        builder.Property(u => u.Name).HasMaxLength(150).IsRequired();

        builder.Property(u => u.Email).HasMaxLength(150).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.Property(u => u.Password).HasMaxLength(150).IsRequired();
        builder.Property(u => u.CreatedDate)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
}
