namespace ChatMicroservice.Application.DTOs.Requests;
public record UpdateChatRoomRequestDTO
(
    string? Name = null,
    bool? IsActive = null,
    List<Guid>? AddParticipantUserIds = null,
    List<Guid>? RemoveParticipantUserIds = null
);

