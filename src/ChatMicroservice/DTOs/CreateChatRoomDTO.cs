using System.ComponentModel.DataAnnotations;

namespace ChatMicroservice.DTOs
{
    public class CreateChatRoomDTO
    {
        [Required]
        public string ChatRoomName { get; set; }
        [Required]
        public List<Guid> MemberIds { get; set; }
        public bool IsGroupChat { get; set; }
    }
}