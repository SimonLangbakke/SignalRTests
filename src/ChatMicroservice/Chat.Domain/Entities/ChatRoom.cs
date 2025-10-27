namespace Chat.Domain.Entities;

public class ChatRoom
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }  // Links to the Event microservice
    public string Name { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public List<Message> Messages { get; set; } = new();
    public List<ChatRoomMember> Members { get; set; } = new();
}