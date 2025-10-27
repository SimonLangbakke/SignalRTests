namespace Chat.Domain.Entities;

public class Message
{
    public Guid Id { get; set; }
    public Guid ChatRoomId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = default!;
    public DateTime SentAt { get; set; }
    public bool IsPinned { get; set; }
    public DateTime? PinnedAt { get; set; }
    public Guid? PinnedByUserId { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation property
    public ChatRoom ChatRoom { get; set; } = default!;
}