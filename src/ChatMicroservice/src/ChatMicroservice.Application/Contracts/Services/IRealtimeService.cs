using ChatMicroservice.Application.DTOs;
namespace ChatMicroservice.Application.Contracts.Services;

public interface IRealtimeService
{
    Task NotifyNewMessage(Guid chatRoomId, MessageDTO message);
    Task NotifyMessageEdited(Guid chatRoomId, MessageDTO message);
    Task NotifyMessageDeleted(Guid chatRoomId, Guid messageId);
    Task NotifyUserJoined(Guid chatRoomId, ChatRoomMemberDTO member);
    Task NotifyUserLeft(Guid chatRoomId, Guid userId);
    //Task NotifyUserTyping(Guid chatRoomId, Guid userId);
    //Task NotifyUserStoppedTyping(Guid chatRoomId, Guid userId);
}
