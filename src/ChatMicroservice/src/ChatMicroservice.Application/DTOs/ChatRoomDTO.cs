namespace ChatMicroservice.Application.DTOs;

public record ChatRoomDTO(
    Guid Id,
    string Name,
    bool IsGroupChat,
    Guid CreatedByUserId,
    DateTime CreatedDate,
    DateTime? LastMessageDate,
    bool IsActive,
    List<MessageDTO> Messages,
    List<ChatRoomMemberDTO> Members
);