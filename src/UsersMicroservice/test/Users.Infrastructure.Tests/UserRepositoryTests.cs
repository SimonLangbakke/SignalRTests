using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Users.Application.Dtos.Requests;
using Users.Infrastructure.Context;
using Users.Application.Exceptions;
using Users.Infrastructure.Repository;

namespace Users.Infrastructure.Tests;


public sealed class UserRepositoryTests : IAsyncLifetime
{
    private readonly TestcontainerDatabase _postgresContainer;
    private UserDbContext _context = null!;
    private UserRepository _repository = null!;

    public UserRepositoryTests()
    {
        _postgresContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "testdb",
                Username = "postgres",
                Password = "postgres"
            })
            .WithImage("postgres:15")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        // Create the extension before EF Core does anything
        await using var conn = new NpgsqlConnection(_postgresContainer.ConnectionString);
        await conn.OpenAsync();
        await using (var cmd = new NpgsqlCommand("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";", conn))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        // Now build EF Core context
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseNpgsql(_postgresContainer.ConnectionString)
            .Options;

        _context = new UserDbContext(options);

        // Now it's safe to create schema
        await _context.Database.EnsureCreatedAsync();

        _repository = new UserRepository(_context);
    }

    public async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync().AsTask();
    }

    /**
     * ================================================================================================================
     * ============================================ T E S T I N G =====================================================
     * ================================================================================================================
     */


    private readonly CreateUserRequestDto _mockUser = new CreateUserRequestDto
    {
        Name = "Mock User",
        Email = "mock@example.com",
        Password = "password"
    };

    [Fact]
    public async Task AddUser_ShouldAddUser()
    {
        var addUser = await _repository.AddUser(_mockUser);
        var fetchUser = await _repository.GetUserByEmail(_mockUser.Email);

        Assert.NotNull(addUser);
        Assert.NotNull(fetchUser.userDto);
        Assert.Equal(addUser.Email, fetchUser.userDto.Email);
    }

    [Fact]
    public async Task GetUserById_ShouldGetUser()
    {
        var addUser = await _repository.AddUser(_mockUser);
        var fetchUser = await _repository.GetUserById(addUser!.Id);

        Assert.NotNull(addUser);
        Assert.NotNull(fetchUser);
        Assert.Equal(addUser.Id, fetchUser.Id);
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateUser()
    {
        var addUser = await _repository.AddUser(_mockUser);
        var updateUser = await _repository.UpdateUser(addUser!.Id, u => u.Name = "Updated Name");
        var allUsers = await _repository.GetAllUsers();

        Assert.Equal(addUser.Id, updateUser!.Id);
        Assert.NotEqual(_mockUser.Name, updateUser.Name);
        Assert.Single(allUsers);
    }

    [Fact]
    public async Task DeleteUser_ShouldDeleteUser()
    {
        var addUser = await _repository.AddUser(_mockUser);
        var allUsers = await _repository.GetAllUsers();
        Assert.Single(allUsers);

        await _repository.DeleteUser(addUser!.Id);

        var noUsers = await _repository.GetAllUsers();
        Assert.Empty(noUsers);
    }

    [Fact]
    public async Task AddDuplicateUser_ShouldThrowException()
    {
        await _repository.AddUser(_mockUser);
        await Assert.ThrowsAsync<DuplicateUserEmailException>(async () => await _repository.AddUser(_mockUser));

        var fetchedUser = await _repository.GetUserByEmail(_mockUser.Email);
        Assert.NotNull(fetchedUser.userDto);

        var allUsers = await _repository.GetAllUsers();
        Assert.Single(allUsers);
    }

    [Fact]
    public async Task MultipleUsers_ShouldHaveDifferentIDs()
    {
        var firstUser = await _repository.AddUser(_mockUser);

        var secondMock = new CreateUserRequestDto
        {
            Name = "Second User",
            Email = "second@example.com",
            Password = "password"
        };

        var secondUser = await _repository.AddUser(secondMock);

        Assert.NotEqual(firstUser!.Id, secondUser!.Id);
    }
}
