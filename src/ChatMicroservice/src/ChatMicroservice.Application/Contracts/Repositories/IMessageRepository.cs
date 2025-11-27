using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Application.DTOs.Requests;

namespace ChatMicroservice.Application.Contracts.Repositories;

public interface IMessageRepository
{
    Task<MessageDTO> SendMessage(SendMessageRequestDTO request, Guid senderId);
    Task<List<MessageDTO>> GetMessages(Guid chatRoomId, int skip = 0, int take = 50);
    Task<MessageDTO?> GetMessageById(Guid id);
    Task<MessageDTO> EditMessage(Guid id, string newContent, Guid userId);
    Task DeleteMessage(Guid id, Guid userId);
    Task<MessageDTO> PinMessage(Guid id, Guid userId);
    Task<MessageDTO> UnpinMessage(Guid id, Guid userId);
    Task<List<MessageDTO>> GetPinnedMessages(Guid chatRoomId);
    Task<int> GetUnreadCount(Guid chatRoomId, Guid userId);
}
