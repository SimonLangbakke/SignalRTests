namespace ChatMicroservice.Application.DTOs;

public record MessageDTO(
    Guid Id,
    Guid ChatRoomId,
    Guid SenderId,
    string Content,
    DateTime SentAt,
    bool IsDeleted
);