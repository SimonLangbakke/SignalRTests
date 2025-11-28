using ChatMicroservice.Application.Contracts.Repositories;
using ChatMicroservice.Application.Contracts.Services;
using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Application.DTOs.Requests;
using ChatMicroservice.Application.Exceptions;

namespace ChatMicroservice.Application.Services;

public class MessageService(
    IMessageRepository messageRepository,
    IChatRoomRepository chatRoomRepository,
    CurrentUser currentUser) : IMessageService
{
    public async Task<MessageDTO> SendMessageAsync(SendMessageRequestDTO request)
    {
        if (!await chatRoomRepository.IsUserMemberOfChatRoom(request.ChatRoomId, currentUser.UserId))
            throw new UnauthorizedChatAccessException(request.ChatRoomId, currentUser.UserId);

        return await messageRepository.SendMessage(request, currentUser.UserId);
    }

    public async Task<List<MessageDTO>> GetMessagesAsync(Guid chatRoomId, int skip = 0, int take = 50)
    {
        if (!await chatRoomRepository.IsUserMemberOfChatRoom(chatRoomId, currentUser.UserId))
            throw new UnauthorizedChatAccessException(chatRoomId, currentUser.UserId);

        return await messageRepository.GetMessages(chatRoomId, skip, take);
    }

    public async Task DeleteMessageAsync(Guid messageId)
    {
        var message = await messageRepository.GetMessageById(messageId)
            ?? throw new MessageNotFoundException(messageId);

        // Only sender can delete their message
        if (message.SenderId != currentUser.UserId)
            throw new UnauthorizedChatAccessException(message.ChatRoomId, currentUser.UserId);

        await messageRepository.DeleteMessage(messageId);
    }
}