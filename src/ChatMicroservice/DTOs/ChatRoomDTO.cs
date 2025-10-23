namespace ChatMicroservice.DTOs
{
    public class ChatRoomDTO
    {
        public Guid ChatRoomId { get; set; }
        public string ChatRoomName { get; set; }
        public bool IsGroupChat { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastMessageDate { get; set; }
        public List<Guid> MemberIds { get; set; }
    }
}