namespace ChatMicroservice.DTOs
{
    public class SendMessageDTO
    {
        [Required]
        public Guid SenderId { get; set; }
        [Required]
        public Guid ChatRoomId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
    }
}