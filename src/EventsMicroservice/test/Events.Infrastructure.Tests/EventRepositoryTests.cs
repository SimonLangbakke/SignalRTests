using Events.Application.Dtos.Common;
using Events.Application.Dtos.Requests;
using Events.Application.Mappers;
using Events.Domain.Entities;
using Events.Infrastructure.Context;
using Events.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Tests;

public sealed class EventRepositoryTests
{
    private static EventsDbContext NewContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<EventsDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var ctx = new EventsDbContext(options);
        ctx.Database.EnsureCreated();
        return ctx;
    }

    private static EventRepository NewRepo(string dbName)
        => new EventRepository(NewContext(dbName));

    private static CreateEventRequestDto NewEventPayload(
        string name = "Mock Event",
        string description = "Desc",
        string foodName = "Pizza",
        int maxAllowedParticipants = 25,
        int minAge = 18,
        int maxAge = 65)
    {
        throw new NotImplementedException("Update tests after changes.");
        // return new CreateEventRequestDto(
        //     Name: name,
        //     Description: description,
        //     MaxAllowedParticipants: maxAllowedParticipants,
        //     MinAllowedAge: minAge,
        //     MaxAllowedAge: maxAge,
        //     StartDate: DateTime.UtcNow.AddDays(7),
        //     ReservationEndDate: DateTime.UtcNow.AddDays(3),
        //     ImageThumbnail: "thumb.png",
        //     IsPublic: true,
        //     new EventFoodDetails(),
        //     Enumerable.Empty<EventImage>()
        // );
    }

    [Fact]
    public async Task AddEvent_ShouldAdd()
    {
        var repo = NewRepo(Guid.NewGuid().ToString());
        var eventDto = NewEventPayload();
        var created = await repo.AddEvent(eventDto.ToEntity(Guid.NewGuid()));
        Assert.NotNull(created);
        var all = await repo.GetAllEvents(new PaginationDto(0, 0));
        Assert.Single(all);
    }

    [Fact]
    public async Task GetEventById_ShouldReturn()
    {
        var repo = NewRepo(Guid.NewGuid().ToString());
        var eventDto = NewEventPayload();
        var created = await repo.AddEvent(eventDto.ToEntity(Guid.NewGuid()));
        var fetched = await repo.GetEventById(created!.EventId);
        Assert.NotNull(fetched);
        Assert.Equal(created.EventId, fetched!.EventId);
    }

    [Fact]
    public async Task UpdateEvent_ShouldModifyFields()
    {
        var repo = NewRepo(Guid.NewGuid().ToString());
        var created = await repo.AddEvent(NewEventPayload().ToEntity(Guid.NewGuid()));
        var updated = await repo.UpdateEvent(created!.EventId, e =>
        {
            e.Name = "Updated Name";
            e.MaxAllowedParticipants = 50;
        });
        Assert.NotNull(updated);
        Assert.Equal("Updated Name", updated!.Name);
        Assert.Equal(50, updated.MaxAllowedParticipants);
    }

    [Fact]
    public async Task DeleteEvent_ShouldRemove()
    {
        var repo = NewRepo(Guid.NewGuid().ToString());
        var created = await repo.AddEvent(NewEventPayload().ToEntity(Guid.NewGuid()));
        var allBefore = await repo.GetAllEvents(new PaginationDto(0, 0));
        Assert.Single(allBefore);
        var deleted = await repo.DeleteEvent(created!.EventId);
        Assert.NotNull(deleted);
        var allAfter = await repo.GetAllEvents(new PaginationDto(0, 0));
        Assert.Empty(allAfter);
    }

    [Fact]
    public async Task GetByUserId_ShouldFilter()
    {
        var db = Guid.NewGuid().ToString();
        var repo = NewRepo(db);

        var u1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var u2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

        await repo.AddEvent(NewEventPayload(name: "User1 Event A").ToEntity(Guid.NewGuid()));
        await repo.AddEvent(NewEventPayload(name: "User1 Event B").ToEntity(Guid.NewGuid()));

        var e3 = await repo.AddEvent(NewEventPayload(name: "User2 Event A").ToEntity(Guid.NewGuid()));
        await repo.UpdateEvent(e3!.EventId, e => e.UserId = u2);

        var user1Events = await repo.GetEventsByUserId(u1);
        var user2Events = await repo.GetEventsByUserId(u2);

        Assert.Equal(2, user1Events.Count);
        Assert.Single(user2Events);
    }
}
