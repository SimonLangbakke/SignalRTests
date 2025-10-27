namespace Chat.Application.DTOs.Requests;

public class SendMessageRequestDto
{
    public Guid ChatRoomId { get; init; }
    public string Content { get; init; } = default!;
}
