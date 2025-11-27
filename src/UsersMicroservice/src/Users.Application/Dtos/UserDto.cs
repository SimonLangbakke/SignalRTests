namespace Users.Application.Dtos;

public record UserDto(Guid Id, string Name, string Email, DateOnly Birthday, DateTime CreatedDate, bool IsActive);
