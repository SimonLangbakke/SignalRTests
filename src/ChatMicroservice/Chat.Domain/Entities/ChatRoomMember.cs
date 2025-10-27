namespace Chat.Domain.Entities;

public class ChatRoomMember
{
    public Guid Id { get; set; }
    public Guid ChatRoomId { get; set; }
    public Guid UserId { get; set; }
    public bool IsHost { get; set; }  // Determines admin privileges for pinning messages etc.
    public DateTime JoinedAt { get; set; }
    public DateTime? LastReadAt { get; set; }

    // Navigation property
    public ChatRoom ChatRoom { get; set; } = default!;
}