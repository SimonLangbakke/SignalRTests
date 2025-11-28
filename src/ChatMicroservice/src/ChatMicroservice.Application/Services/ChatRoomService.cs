using ChatMicroservice.Application.Contracts.Repositories;
using ChatMicroservice.Application.Contracts.Services;
using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Application.DTOs.Requests;
using ChatMicroservice.Application.Exceptions;

namespace ChatMicroservice.Application.Services
{
    public class ChatRoomService(IChatRoomRepository chatRoomRepository, CurrentUser currentUser) : IChatRoomService
    {
        public async Task<ChatRoomDTO> CreateChatRoomAsync(CreateChatRoomRequestDTO request)
        {
            return await chatRoomRepository.CreateChatRoomAsync(request, currentUser.UserId);
        }

        public async Task DeleteChatRoomAsync(Guid chatRoomId)
        {
            if (!await chatRoomRepository.IsUserAdminOfChatRoom(chatRoomId, currentUser.UserId))
                throw new UnauthorizedChatAccessException(chatRoomId, currentUser.UserId);

            await chatRoomRepository.DeleteChatRoomAsync(chatRoomId);
        }

        public async Task<ChatRoomDTO?> GetChatRoomByIdAsync(Guid chatRoomId)
        {
            var chatRoom = await chatRoomRepository.GetChatRoomByIdAsync(chatRoomId)
           ?? throw new ChatRoomNotFoundException(chatRoomId);

            if (!await chatRoomRepository.IsUserMemberOfChatRoom(chatRoomId, currentUser.UserId))
                throw new UnauthorizedChatAccessException(chatRoomId, currentUser.UserId);

            return chatRoom;
        }

        public async Task<List<ChatRoomDTO>> GetUserChatRoomsAsync()
        {
            return await chatRoomRepository.GetChatRoomsByUserId(currentUser.UserId);
        }

        public async Task<ChatRoomDTO> UpdateChatRoomAsync(Guid chatRoomId, UpdateChatRoomRequestDTO request)
        {
            if (!await chatRoomRepository.IsUserAdminOfChatRoom(chatRoomId, currentUser.UserId))
                throw new UnauthorizedChatAccessException(chatRoomId, currentUser.UserId);

            return await chatRoomRepository.UpdateChatRoomAsync(chatRoomId, request);
        }
    }
}
