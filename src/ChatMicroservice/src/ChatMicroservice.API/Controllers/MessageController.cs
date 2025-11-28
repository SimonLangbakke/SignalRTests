using ChatMicroservice.Application.Contracts.Services;
using ChatMicroservice.Application.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatMicroservice.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MessageController(IMessageService messageService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequestDTO request)
    {
        var message = await messageService.SendMessageAsync(request);
        return Ok(message);
    }

    [HttpGet("chatroom/{chatRoomId}")]
    public async Task<IActionResult> GetMessages([FromRoute] Guid chatRoomId, [FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var messages = await messageService.GetMessagesAsync(chatRoomId, skip, take);
        return Ok(messages);
    }

    [HttpDelete("{messageId}")]
    public async Task<IActionResult> DeleteMessage([FromRoute] Guid messageId)
    {
        await messageService.DeleteMessageAsync(messageId);
        return NoContent();
    }
}