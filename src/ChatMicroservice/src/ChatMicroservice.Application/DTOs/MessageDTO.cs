namespace ChatMicroservice.Application.DTOs;

public record MessageDTO(

    Guid Id,
    Guid ChatRoomId,
    Guid SenderId,
    string Content,
    DateTime SentAt,
    bool IsEdited,
    DateTime? EditedAt,
    bool IsDeleted,
    bool IsPinned,
    bool IsOwnMessage

);
