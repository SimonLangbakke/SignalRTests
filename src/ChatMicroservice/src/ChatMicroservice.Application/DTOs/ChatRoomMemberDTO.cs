namespace ChatMicroservice.Application.DTOs;

public record ChatRoomMemberDTO(
    Guid Id,
    Guid ChatRoomId,
    Guid UserId,
    DateTime JoinedDate,
    bool IsActive,
    bool IsAdmin
);