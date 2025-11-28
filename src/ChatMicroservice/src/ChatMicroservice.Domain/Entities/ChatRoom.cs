namespace ChatMicroservice.Domain.Entities;

public class ChatRoom
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsGroupChat { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastMessageDate { get; set; }
    public bool IsActive { get; set; } = true;

    public List<ChatMessage> Messages { get; set; } = new();
    public List<ChatRoomMember> Members { get; set; } = new();
}