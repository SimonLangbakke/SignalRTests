using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Domain.Entities;

namespace ChatMicroservice.Application.Mappers;

public static class ChatRoomMapper
{
    public static ChatRoomDTO ToDto(this ChatRoom chatRoom)
    {
        return new ChatRoomDTO(
            chatRoom.Id,
            chatRoom.Name,
            chatRoom.IsGroupChat,
            chatRoom.CreatedByUserId,
            chatRoom.CreatedDate,
            chatRoom.LastMessageDate,
            chatRoom.IsActive,
            chatRoom.Messages?.Select(m => m.ToDto()).ToList() ?? [],
            chatRoom.Members?.Select(m => m.ToDto()).ToList() ?? []
        );
    }
}