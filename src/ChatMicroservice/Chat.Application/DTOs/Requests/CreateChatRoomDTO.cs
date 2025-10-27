namespace Chat.Application.DTOs.Requests;

public class CreateChatRoomRequestDto
{
    public Guid EventId { get; init; }
    public string Name { get; init; } = default!;
    public List<Guid> InitialMemberIds { get; init; } = new();
}