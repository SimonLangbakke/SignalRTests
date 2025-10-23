namespace ChatMicroservice.Models
{
    public class ChatRoomMember
    {
        public Guid Id { get; set; }
        public Guid ChatRoomId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime LastActiveDate { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}