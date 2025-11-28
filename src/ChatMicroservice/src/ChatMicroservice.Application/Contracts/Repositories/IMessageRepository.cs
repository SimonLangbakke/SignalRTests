using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Application.DTOs.Requests;

namespace ChatMicroservice.Application.Contracts.Repositories;

public interface IMessageRepository
{
    Task<MessageDTO> SendMessage(SendMessageRequestDTO request, Guid senderId);
    Task<List<MessageDTO>> GetMessages(Guid chatRoomId, int skip = 0, int take = 50);
    Task<MessageDTO?> GetMessageById(Guid id);
    Task DeleteMessage(Guid id);
}