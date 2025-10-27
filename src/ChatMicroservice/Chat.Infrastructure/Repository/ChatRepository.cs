using Microsoft.EntityFrameworkCore;

namespace ChatMicroservice.Repository
{
    using ChatMicroservice.Models;
    using ChatMicroservice.Data;
    using ChatMicroservice.DTOs;

    public class ChatRepository : IChatRepository
    {
        private readonly ChatDbContext _context;

        public ChatRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<ChatRoom> CreateChatRoomAsync(CreateChatRoomDTO createChatRoomDTO)
        {
            var chatRoom = new ChatRoom
            {
                Id = Guid.NewGuid(),
                Name = createChatRoomDTO.ChatRoomName,
                IsGroupChat = createChatRoomDTO.IsGroupChat,
                Members = createChatRoomDTO.MemberIds.Select(id => new ChatRoomMember
                {
                    UserId = id
                }).ToList()
            };

            _context.ChatRooms.Add(chatRoom);
            await _context.SaveChangesAsync();
            return chatRoom;
        }

        public async Task<ChatRoom> GetChatRoomByIdAsync(Guid chatRoomId)
        {
            return await _context.ChatRooms
                .Include(cr => cr.Members)
                .Include(cr => cr.Messages)
                .FirstOrDefaultAsync(cr => cr.Id == chatRoomId);
        }

        public async Task<List<ChatRoom>> GetChatRoomsForUserAsync(Guid userId)
        {
            return await _context.ChatRooms
                .Where(cr => cr.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        public async Task<bool> IsUserInChatRoomAsync(Guid chatRoomId, Guid userId)
        {
            return await _context.ChatRoomMembers
                .AnyAsync(m => m.ChatRoomId == chatRoomId && m.UserId == userId);
        }

        public async Task RemoveUserFromChatRoomAsync(Guid chatRoomId, Guid userId)
        {
            var member = await _context.ChatRoomMembers
                .FirstOrDefaultAsync(m => m.ChatRoomId == chatRoomId && m.UserId == userId);
            if (member != null)
            {
                _context.ChatRoomMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddUserToChatRoomAsync(Guid chatRoomId, ChatRoomMember member)
        {
            member.ChatRoomId = chatRoomId;
            _context.ChatRoomMembers.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task AddMessageToChatRoomAsync(Guid chatRoomId, Message message)
        {
            message.ChatRoomId = chatRoomId;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetMessagesForChatRoomAsync(Guid chatRoomId, int limit, int offset)
        {
            return await _context.Messages
                .Where(m => m.ChatRoomId == chatRoomId)
                .OrderByDescending(m => m.SentDate)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<List<ChatRoomMember>> GetChatRoomMembersAsync(Guid chatRoomId)
        {
            return await _context.ChatRoomMembers
                .Where(m => m.ChatRoomId == chatRoomId)
                .ToListAsync();
        }

        public async Task<ChatRoomMember> GetChatRoomMemberAsync(Guid chatRoomId, Guid userId)
        {
            return await _context.ChatRoomMembers
                .FirstOrDefaultAsync(m => m.ChatRoomId == chatRoomId && m.UserId == userId);
        }

        public async Task<int> GetUnreadMessageCountAsync(Guid chatRoomId, Guid userId)
        {
            return await _context.Messages
                .Where(m => m.ChatRoomId == chatRoomId && !m.IsRead && m.SenderId != userId)
                .CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}