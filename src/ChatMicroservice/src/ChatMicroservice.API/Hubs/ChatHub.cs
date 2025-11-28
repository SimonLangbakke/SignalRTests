using ChatMicroservice.Application.Contracts.Services;
using ChatMicroservice.Application.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatMicroservice.API.Hubs;

[Authorize]
public class ChatHub(IMessageService messageService, IChatRoomService chatRoomService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        // Auto-join user's chat rooms
        var chatRooms = await chatRoomService.GetUserChatRoomsAsync();
        foreach (var room in chatRooms)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id.ToString());
        }
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(Guid chatRoomId, string content)
    {
        var request = new SendMessageRequestDTO(chatRoomId, content);
        var message = await messageService.SendMessageAsync(request);
        await Clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", message);
    }

    public async Task JoinRoom(Guid chatRoomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());
        await Clients.Caller.SendAsync("JoinedRoom", chatRoomId);
    }

    public async Task LeaveRoom(Guid chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
    }

    public async Task StartTyping(Guid chatRoomId)
    {
        var userId = GetUserId();
        await Clients.OthersInGroup(chatRoomId.ToString()).SendAsync("UserTyping", userId);
    }

    public async Task StopTyping(Guid chatRoomId)
    {
        var userId = GetUserId();
        await Clients.OthersInGroup(chatRoomId.ToString()).SendAsync("UserStoppedTyping", userId);
    }

    private Guid GetUserId()
    {
        var claim = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new HubException("User not authenticated");
        return Guid.Parse(claim);
    }
}