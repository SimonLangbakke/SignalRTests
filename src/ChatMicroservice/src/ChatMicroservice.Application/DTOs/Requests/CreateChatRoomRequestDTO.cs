namespace ChatMicroservice.Application.DTOs.Requests;
public record CreateChatRoomRequestDTO
(
    string Name,
    List<Guid> ParticipantUserIds,
    bool IsGroupChat
);