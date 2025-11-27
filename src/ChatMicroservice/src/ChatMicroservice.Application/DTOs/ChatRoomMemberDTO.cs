namespace ChatMicroservice.Application.DTOs;
public record ChatRoomMemberDTO(
    Guid UserId,
    string UserName,
    bool IsAdmin,
    DateTime JoinedAt,
    //DateTime? LastReadAt,
    bool IsActive
);