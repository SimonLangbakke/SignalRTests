using ChatMicroservice.Application.Contracts.Repositories;
using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Application.DTOs.Requests;
using ChatMicroservice.Application.Exceptions;
using ChatMicroservice.Application.Mappers;
using ChatMicroservice.Domain.Entities;
using ChatMicroservice.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ChatMicroservice.Infrastructure.Repository;

public class MessageRepository(ChatDbContext context) : IMessageRepository
{
    public async Task<MessageDTO> SendMessage(SendMessageRequestDTO request, Guid senderId)
    {
        var message = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ChatRoomId = request.ChatRoomId,
            SenderUserId = senderId,
            Content = request.Content,
            SentDate = DateTime.UtcNow,
            IsDeleted = false
        };

        context.Messages.Add(message);

        // Update last message date
        var chatRoom = await context.ChatRooms.FindAsync(request.ChatRoomId);
        if (chatRoom != null)
            chatRoom.LastMessageDate = message.SentDate;

        await context.SaveChangesAsync();
        return message.ToDto();
    }

    public async Task<List<MessageDTO>> GetMessages(Guid chatRoomId, int skip = 0, int take = 50)
    {
        var messages = await context.Messages
            .Where(m => m.ChatRoomId == chatRoomId && !m.IsDeleted)
            .OrderByDescending(m => m.SentDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return messages.Select(m => m.ToDto()).ToList();
    }

    public async Task<MessageDTO?> GetMessageById(Guid id)
    {
        var message = await context.Messages
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

        return message?.ToDto();
    }

    public async Task DeleteMessage(Guid id)
    {
        var message = await context.Messages
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted)
            ?? throw new MessageNotFoundException(id);

        message.IsDeleted = true;
        await context.SaveChangesAsync();
    }
}