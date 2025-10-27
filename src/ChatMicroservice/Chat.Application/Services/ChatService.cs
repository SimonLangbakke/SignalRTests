using System.Runtime.CompilerServices;

namespace Chat.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
            _userService = userService;

        }

        public async Task<ChatRoomDto> CreateChatRoomAsync(Guid userId, string userName, CreateChatRoomDto dto)
        {
            var allUserIds = dto.MemberIds.Concat(new List<Guid> { userId }).Distinct().ToList();
            var userDetails = await _userService.GetUsersByIdsAsync(allUserIds);

            var chatRoom = new ChatRoom
            {
                Id = Guid.NewGuid(),
                Name = dto.ChatRoomName,
                IsGroupChat = dto.IsGroupChat,
                CreatedDate = DateTime.UtcNow,
                Members = new List<ChatRoomMember>
                {
                    new ChatRoomMember
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        UserName = userName,
                        JoinedDate = DateTime.UtcNow
                    }
                }
            };

            foreach (var memberId in dto.MemberIds)
            {
                var userInfo = userDetails.GetValueOrDefault(memberId);

                var memberName = userInfo?.Displayname
                ?? userInfo?.Username
                ?? "Unknown";

                chatRoom.Members.Add(new ChatRoomMember
                {
                    Id = Guid.NewGuid(),
                    UserId = memberId,
                    UserName = memberName,
                    JoinedDate = DateTime.UtcNow
                });
            }
            await _chatRepository.AddChatRoomAsync(chatRoom);
            return MapToDto(chatRoom);
        }


    }
}