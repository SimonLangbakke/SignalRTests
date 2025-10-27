namespace ChatMicroservice.Data.Repositories
{
    public interface IChatRepository
    {
        Task<ChatRoom> CreateChatRoomAsync(ChatRoom chatRoom, List<ChatRoomMember> members);
        Task<ChatRoom> GetChatRoomByIdAsync(Guid chatRoomId);
        Task<List<ChatRoom>> GetChatRoomsForUserAsync(Guid userId);
        Task<bool> IsUserInChatRoomAsync(Guid chatRoomId, Guid userId);
        Task RemoveUserFromChatRoomAsync(Guid chatRoomId, Guid userId);
        Task AddUserToChatRoomAsync(Guid chatRoomId, ChatRoomMember member);

        Task AddMessageToChatRoomAsync(Guid chatRoomId, Message message);
        Task<List<Message>> GetMessagesForChatRoomAsync(Guid chatRoomId, int limit, int offset);

        Task<List<ChatRoomMember>> GetChatRoomMembersAsync(Guid chatRoomId);
        Task<ChatRoomMember> GetChatRoomMemberAsync(Guid chatRoomId, Guid userId);
        Task<int> GetUnreadMessageCountAsync(Guid chatRoomId, Guid userId);

        Task SaveChangesAsync();
    }
}