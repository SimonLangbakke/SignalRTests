using Microsoft.AspNetCore.Mvc;
using Users.Application.Contracts.Services;
using Users.Application.Dtos.Requests;

namespace Users.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService _userService) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequestDto request)
    {
        var token = await _userService.AuthenticateAndGenerateUserTokenAsync(request);
        return Ok(token);
    }

    [HttpPost]
    [ProducesResponseType<Guid>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUser([FromBody] CreateUserRequestDto request)
    {
        var userId = await _userService.CreateUserAsync(request);
        return Ok(userId);
    }

    [HttpGet("multiple-users")]
    public async Task<IActionResult> GetMultipleUsersById([FromQuery] IEnumerable<Guid> ids)
    {
        var users = await _userService.GetMultipleUsersByIdsAsync(ids);

        return Ok(users);
    }

    [HttpGet()]
    public async Task<IActionResult> GetUserById([FromQuery] Guid id)
    {
        var users = await _userService.GetUserByIdAsync(id);

        return Ok(users);
    }

    [HttpPatch("{id}/deactivate-user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeactivateUser([FromRoute] Guid id)
    {
        await _userService.DeactivateUserAsync(id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}
