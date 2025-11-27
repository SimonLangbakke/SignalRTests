namespace ChatMicroservice.Domain.Entities
{
    public class ChatRoomMember
    {
        public Guid Id { get; set; }
        public Guid ChatRoomId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinedDate { get; set; }
        public bool isActive { get; set; } = true;
        public ChatRoom ChatRoom { get; set; } = default!;
    }
}