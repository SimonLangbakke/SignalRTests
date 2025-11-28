using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Application.DTOs.Requests;

namespace ChatMicroservice.Application.Contracts.Repositories;

public interface IChatRoomRepository
{
    Task<ChatRoomDTO?> GetChatRoomByIdAsync(Guid chatRoomId);
    Task<List<ChatRoomDTO>> GetChatRoomsByUserId(Guid userId);
    Task<ChatRoomDTO> CreateChatRoomAsync(CreateChatRoomRequestDTO request, Guid createdByUserId);
    Task<ChatRoomDTO> UpdateChatRoomAsync(Guid chatRoomId, UpdateChatRoomRequestDTO request);
    Task DeleteChatRoomAsync(Guid chatRoomId);
    Task<bool> IsUserAdminOfChatRoom(Guid chatRoomId, Guid userId);
    Task<bool> IsUserMemberOfChatRoom(Guid chatRoomId, Guid userId);
}