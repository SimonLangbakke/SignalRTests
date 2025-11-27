namespace ChatMicroservice.Application.DTOs;
public record ChatRoomDTO(

    Guid Id, 
    string Name, 
    bool isGroupChat,
    Guid CreatedByUserId,
    DateTime CreatedDate,
    DateTime? LastMessageDate,
    bool isActive,
    List<MessageDTO> Messages,
    List<ChatRoomMemberDTO> Members
);