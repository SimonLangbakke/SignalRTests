namespace ChatMicroservice.Application.DTOs.Requests;
public record SendMessageRequestDTO
(
    Guid ChatRoomId,
    string Content
);
