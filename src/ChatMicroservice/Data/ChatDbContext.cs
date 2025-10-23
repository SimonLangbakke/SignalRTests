namespace ChatMicroservice.Data
{
    using ChatMicroservice.Models;
    using Microsoft.EntityFrameworkCore;

    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }

        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatRoomMember> ChatRoomMembers { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatRoom>()
                .HasMany(cr => cr.Members)
                .WithOne(m => m.ChatRoom)
                .HasForeignKey(m => m.ChatRoomId);

            modelBuilder.Entity<ChatRoom>()
                .HasMany(cr => cr.Messages)
                .WithOne(m => m.Chatroom)
                .HasForeignKey(m => m.ChatRoomId);

            modelBuilder.Entity<ChatRoomMember>()
                .HasIndex(m => new { m.ChatRoomId, m.UserId }).IsUnique();

            modelBuilder.Entity<Message>().HasIndex(m => new { m.ChatRoomId, m.SentDate });
        }
    }
}