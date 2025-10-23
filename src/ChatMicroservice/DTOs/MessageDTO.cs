namespace ChatMicroservice.DTOs
{
    public class MessageDTO
    {
        public Guid MessageId { get; set; }
        public Guid ChatRoomId { get; set; }
        public Guid SenderId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsOwnedByCurrentUser { get; set; }
    }
}