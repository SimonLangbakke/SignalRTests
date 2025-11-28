using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Application.DTOs.Requests;

namespace ChatMicroservice.Application.Contracts.Services;

public interface IChatRoomService
{
    Task<ChatRoomDTO> CreateChatRoomAsync(CreateChatRoomRequestDTO request);
    Task<ChatRoomDTO?> GetChatRoomByIdAsync(Guid chatRoomId);
    Task<List<ChatRoomDTO>> GetUserChatRoomsAsync();
    Task<ChatRoomDTO> UpdateChatRoomAsync(Guid chatRoomId, UpdateChatRoomRequestDTO request);
    Task DeleteChatRoomAsync(Guid chatRoomId);
}