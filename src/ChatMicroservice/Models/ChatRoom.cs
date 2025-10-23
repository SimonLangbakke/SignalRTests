namespace ChatMicroservice.Models
{
    public class ChatRoom
    {
        public Guid ChatRoomId { get; set; }
        public string ChatRoomName { get; set; }
        public bool IsGroupChat { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastMessageDate { get; set; }
        List<Guid> MemberIds { get; set; } = new();
        List<Message> Messages { get; set; } = new();
    }
}