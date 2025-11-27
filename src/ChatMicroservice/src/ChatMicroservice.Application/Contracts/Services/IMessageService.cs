using ChatMicroservice.Application.DTOs;
using ChatMicroservice.Application.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMicroservice.Application.Contracts.Services;

public interface IMessageService
{
    Task<MessageDTO> SendMessageAsync(SendMessageRequestDTO request);
    Task<List<MessageDTO>> GetMessagesAsync(Guid chatRoomId, int skip = 0, int take = 50);
    Task DeleteMessageAsync(Guid messageId);
    Task MarkAsReadAsync(Guid chatRoomId);
    //Task<MessageDTO> EditMessageAsync(Guid messageId, string newContent);
    //Task<MessageDTO> PinMessageAsync(Guid messageId);
    //Task<MessageDTO> UnpinMessageAsync(Guid messageId);
    //Task<List<MessageDTO>> GetPinnedMessagesAsync(Guid chatRoomId);
    //Task<int> GetUnreadCountAsync(Guid chatRoomId);
}
