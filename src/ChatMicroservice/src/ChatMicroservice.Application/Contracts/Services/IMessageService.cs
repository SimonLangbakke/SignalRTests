using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Application.DTOs.Requests;

namespace ChatMicroservice.Application.Contracts.Services;

public interface IMessageService
{
    Task<MessageDTO> SendMessageAsync(SendMessageRequestDTO request);
    Task<List<MessageDTO>> GetMessagesAsync(Guid chatRoomId, int skip = 0, int take = 50);
    Task DeleteMessageAsync(Guid messageId);
}
