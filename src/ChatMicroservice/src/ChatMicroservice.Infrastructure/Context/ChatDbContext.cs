using ChatMicroservice.Domain.Entities;
using ChatMicroservice.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ChatMicroservice.Infrastructure.Context;

public class ChatDbContext : DbContext
{
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatMessage> Messages { get; set; }
    public DbSet<ChatRoomMember> ChatRoomMembers { get; set; }

    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChatRoomEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ChatMessageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ChatRoomMemberEntityConfiguration());
    }
}