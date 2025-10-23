namespace ChatMicroservice.Services
{
    public interface IChatService
    {
        Task<ChatRoomDto> CreateChatRoomAsync(Guid userId, string userName, CreateChatRoomDto dto);
        Task<List<ChatRoomDto>> GetUserChatRoomsAsync(Guid userId);
        Task<List<MessageDto>> GetMessagesAsync(Guid userId, Guid chatRoomId, int skip, int take);
        Task<MessageDto> SendMessageAsync(Guid userId, string userName, SendMessageDto dto);
        Task DeleteMessageAsync(Guid userId, Guid messageId);
        Task MarkAsReadAsync(Guid userId, Guid chatRoomId);
    }
}