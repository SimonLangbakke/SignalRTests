namespace Chat.Application.DTOs;

public record MessageDto(
    Guid Id,
    Guid SenderId,
    string SenderName,  // Enriched from UsersAPI
    string Content,
    DateTime SentAt,
    bool IsPinned,
    bool CanPin);  // Based on whether requesting user is host