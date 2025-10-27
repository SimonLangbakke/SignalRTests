using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace Chat.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IMessageService messageService, ILogger<ChatHub> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    public async Task SendMessage(SendMessageRequestDto dto)
    {
        try
        {
            var message = await _messageService.SendMessageAsync(dto);

            // Broadcast to all members of chat room
            await Clients.Group($"room_{dto.ChatRoomId}").SendAsync("ReceiveMessage", message);
        }
        catch (RequestValidationException ex)
        {
            // Send validation errors to caller only
            await Clients.Caller.SendAsync("ValidationError", ex.Errors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to room {RoomId}", dto.ChatRoomId);
            await Clients.Caller.SendAsync("Error", "Failed to send message");
        }
    }

    public async Task JoinRoom(Guid chatRoomId)
    {
        var userId = GetUserId();

        // Add to SignalR group for this chat room
        await Groups.AddToGroupAsync(Context.ConnectionId, $"room_{chatRoomId}");

        _logger.LogInformation("User {UserId} joined room {RoomId}", userId, chatRoomId);
    }

    public async Task LeaveRoom(Guid chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"room_{chatRoomId}");
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        _logger.LogInformation("User {UserId} connected to chat", userId);
        await base.OnConnectedAsync();
    }

    private Guid GetUserId()
    {
        var claim = Context.User?.FindFirst("sub")?.Value;
        return Guid.Parse(claim ?? throw new UnauthorizedException());
    }
}