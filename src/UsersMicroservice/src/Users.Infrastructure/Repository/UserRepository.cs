using Microsoft.EntityFrameworkCore;
using Users.Application.Contracts.Repositories;
using Users.Application.Dtos;
using Users.Application.Dtos.Requests;
using Users.Application.Mappers;
using Users.Domain.Entities;
using Users.Infrastructure.Context;
using Users.Application.Exceptions;

namespace Users.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id)
            ?? throw new UserIdNotFoundException(id);
        return user.ToDto();
    }

    public async Task<(UserDto userDto, string hashedPassword)> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email)
            ?? throw new WrongUserCredentialsException(email);
        return (user.ToDto(), user.Password);
    }

    public async Task<List<UserDto>> GetAllUsers()
    {
        return await _context.Users.Select(x => x.ToDto()).ToListAsync();
    }

    public async Task<UserDto> AddUser(CreateUserRequestDto createUser)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        int age = today.Year - createUser.Birthday.Year;

        var user = new User
        {
            Name = createUser.Name,
            Email = createUser.Email,
            Password = createUser.Password,
            Birthday = createUser.Birthday,
            Age = age
        };

        var exists = await _context.Users.AnyAsync(u => u.Email == createUser.Email);
        if (exists) throw new DuplicateUserEmailException(createUser.Email);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user.ToDto();
    }

    public async Task DeleteUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id)
                   ?? throw new UserIdNotFoundException(id);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserDto> UpdateUser(Guid id, Action<User> op)
    {
        var user = await _context.Users.FindAsync(id)
            ?? throw new UserIdNotFoundException(id);

        op(user);
        await _context.SaveChangesAsync();
        return user.ToDto();
    }

    public async Task DeactivateUser(Guid id)
    {
        var user = await _context.Users.FindAsync(id)
           ?? throw new UserIdNotFoundException(id);

        user.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserDto>> GetMultipleUsersByIds(IEnumerable<Guid> ids)
    {
        return await _context.Users
            .Where(x => ids.Contains(x.Id))
            .Select(x => x.ToDto())
            .ToListAsync();
    }
}