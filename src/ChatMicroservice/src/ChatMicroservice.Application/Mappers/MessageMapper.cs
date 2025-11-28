using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Domain.Entities;

namespace ChatMicroservice.Application.Mappers;

public static class MessageMapper
{
    public static MessageDTO ToDto(this ChatMessage message)
    {
        return new MessageDTO(
            message.Id,
            message.ChatRoomId,
            message.SenderUserId,
            message.Content,
            message.SentDate,
            message.IsDeleted
        );
    }
}