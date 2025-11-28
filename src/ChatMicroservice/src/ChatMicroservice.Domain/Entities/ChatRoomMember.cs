namespace ChatMicroservice.Domain.Entities;

public class ChatRoomMember
{
    public Guid Id { get; set; }
    public Guid ChatRoomId { get; set; }
    public Guid UserId { get; set; }
    public DateTime JoinedDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsAdmin { get; set; } = false;

    public ChatRoom ChatRoom { get; set; } = default!;
}