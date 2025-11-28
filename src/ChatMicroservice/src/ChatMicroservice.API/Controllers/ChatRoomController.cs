using ChatMicroservice.Application.Contracts.Services;
using ChatMicroservice.Application.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatMicroservice.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatRoomController(IChatRoomService chatRoomService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomRequestDTO request)
    {
        var chatRoom = await chatRoomService.CreateChatRoomAsync(request);
        return Ok(chatRoom);
    }

    [HttpGet("{chatRoomId}")]
    public async Task<IActionResult> GetChatRoom([FromRoute] Guid chatRoomId)
    {
        var chatRoom = await chatRoomService.GetChatRoomByIdAsync(chatRoomId);
        return Ok(chatRoom);
    }

    [HttpGet]
    public async Task<IActionResult> GetUserChatRooms()
    {
        var chatRooms = await chatRoomService.GetUserChatRoomsAsync();
        return Ok(chatRooms);
    }

    [HttpPut("{chatRoomId}")]
    public async Task<IActionResult> UpdateChatRoom([FromRoute] Guid chatRoomId, [FromBody] UpdateChatRoomRequestDTO request)
    {
        var chatRoom = await chatRoomService.UpdateChatRoomAsync(chatRoomId, request);
        return Ok(chatRoom);
    }

    [HttpDelete("{chatRoomId}")]
    public async Task<IActionResult> DeleteChatRoom([FromRoute] Guid chatRoomId)
    {
        await chatRoomService.DeleteChatRoomAsync(chatRoomId);
        return NoContent();
    }
}