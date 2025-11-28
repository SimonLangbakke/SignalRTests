namespace ChatMicroservice.Domain.Entities;

public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid ChatRoomId { get; set; }
    public Guid SenderUserId { get; set; }
    public string Content { get; set; } = default!;
    public DateTime SentDate { get; set; }
    public bool IsDeleted { get; set; } = false;

    public ChatRoom ChatRoom { get; set; } = default!;
}