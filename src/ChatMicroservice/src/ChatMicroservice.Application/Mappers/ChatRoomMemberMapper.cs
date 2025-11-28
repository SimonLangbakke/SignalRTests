using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Domain.Entities;

namespace ChatMicroservice.Application.Mappers;

public static class ChatRoomMemberMapper
{
    public static ChatRoomMemberDTO ToDto(this ChatRoomMember member)
    {
        return new ChatRoomMemberDTO(
            member.Id,
            member.ChatRoomId,
            member.UserId,
            member.JoinedDate,
            member.IsActive,
            member.IsAdmin
        );
    }
}