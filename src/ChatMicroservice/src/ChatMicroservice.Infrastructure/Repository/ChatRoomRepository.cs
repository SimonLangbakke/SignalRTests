using ChatMicroservice.Application.Contracts.Repositories;
using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Application.DTOs.Requests;
using ChatMicroservice.Application.Exceptions;
using ChatMicroservice.Application.Mappers;
using ChatMicroservice.Domain.Entities;
using ChatMicroservice.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ChatMicroservice.Infrastructure.Repository;

public class ChatRoomRepository(ChatDbContext context) : IChatRoomRepository
{
    public async Task<ChatRoomDTO?> GetChatRoomByIdAsync(Guid chatRoomId)
    {
        var chatRoom = await context.ChatRooms
            .Include(c => c.Members.Where(m => m.IsActive))
            .Include(c => c.Messages.Where(m => !m.IsDeleted).OrderByDescending(m => m.SentDate).Take(50))
            .FirstOrDefaultAsync(c => c.Id == chatRoomId && c.IsActive);

        return chatRoom?.ToDto();
    }

    public async Task<List<ChatRoomDTO>> GetChatRoomsByUserId(Guid userId)
    {
        var chatRooms = await context.ChatRooms
            .Include(c => c.Members)
            .Where(c => c.IsActive && c.Members.Any(m => m.UserId == userId && m.IsActive))
            .OrderByDescending(c => c.LastMessageDate ?? c.CreatedDate)
            .ToListAsync();

        return chatRooms.Select(c => c.ToDto()).ToList();
    }

    public async Task<ChatRoomDTO> CreateChatRoomAsync(CreateChatRoomRequestDTO request, Guid createdByUserId)
    {
        var chatRoom = new ChatRoom
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            IsGroupChat = request.IsGroupChat,
            CreatedByUserId = createdByUserId,
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        context.ChatRooms.Add(chatRoom);

        // Add creator as admin
        context.ChatRoomMembers.Add(new ChatRoomMember
        {
            Id = Guid.NewGuid(),
            ChatRoomId = chatRoom.Id,
            UserId = createdByUserId,
            JoinedDate = DateTime.UtcNow,
            IsActive = true,
            IsAdmin = true
        });

        // Add other participants
        foreach (var participantId in request.ParticipantUserIds.Where(id => id != createdByUserId))
        {
            context.ChatRoomMembers.Add(new ChatRoomMember
            {
                Id = Guid.NewGuid(),
                ChatRoomId = chatRoom.Id,
                UserId = participantId,
                JoinedDate = DateTime.UtcNow,
                IsActive = true,
                IsAdmin = false
            });
        }

        await context.SaveChangesAsync();
        return chatRoom.ToDto();
    }

    public async Task<ChatRoomDTO> UpdateChatRoomAsync(Guid chatRoomId, UpdateChatRoomRequestDTO request)
    {
        var chatRoom = await context.ChatRooms
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == chatRoomId && c.IsActive)
            ?? throw new ChatRoomNotFoundException(chatRoomId);

        if (!string.IsNullOrWhiteSpace(request.Name))
            chatRoom.Name = request.Name;

        await context.SaveChangesAsync();
        return chatRoom.ToDto();
    }

    public async Task DeleteChatRoomAsync(Guid chatRoomId)
    {
        var chatRoom = await context.ChatRooms
            .FirstOrDefaultAsync(c => c.Id == chatRoomId && c.IsActive)
            ?? throw new ChatRoomNotFoundException(chatRoomId);

        chatRoom.IsActive = false;
        await context.SaveChangesAsync();
    }

    public async Task<bool> IsUserAdminOfChatRoom(Guid chatRoomId, Guid userId)
    {
        return await context.ChatRoomMembers
            .AnyAsync(m => m.ChatRoomId == chatRoomId && m.UserId == userId && m.IsActive && m.IsAdmin);
    }

    public async Task<bool> IsUserMemberOfChatRoom(Guid chatRoomId, Guid userId)
    {
        return await context.ChatRoomMembers
            .AnyAsync(m => m.ChatRoomId == chatRoomId && m.UserId == userId && m.IsActive);
    }
}