using Microsoft.EntityFrameworkCore;
using Chat.Domain.Entities;
using Chat.Infrastructure.Context.Chat.Infrastructure.EntityConfigurations;

namespace Chat.Infrastructure.Context;

public class ChatDbContext : DbContext
{
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatRoomMember> ChatRoomMembers { get; set; }

    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChatRoomEntityConfiguration());
        modelBuilder.ApplyConfiguration(new MessageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ChatRoomMemberEntityConfiguration());
    }
}

// Chat.Infrastructure/EntityConfigurations/MessageEntityConfiguration.cs
namespace Chat.Infrastructure.EntityConfigurations;

public class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.Property(m => m.Content)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(m => m.SentAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Index for efficient message retrieval
        builder.HasIndex(m => new { m.ChatRoomId, m.SentAt });

        // Index for pinned messages
        builder.HasIndex(m => new { m.ChatRoomId, m.IsPinned })
            .HasFilter("\"IsPinned\" = true");
    }
}