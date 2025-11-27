using Events.Application.Builders;
using Events.Application.Contracts.ExternalApis;
using Events.Application.Contracts.Repositories;
using Events.Application.Contracts.Services;
using Events.Application.Dtos;
using Events.Application.Dtos.Common;
using Events.Application.Dtos.Requests;
using Events.Application.Exceptions;
using Events.Application.Mappers;
using Events.Domain.Enums;

namespace Events.Application.Services;

public class EventService(
    IEventRepository eventRepository,
    CurrentUser currentUser,
    IUserApiService userApiService,
    IPaymentService paymentService) : IEventService
{
    // Helper methods to avoid code duplication
    private async Task EnsureThatUserOwnsTheEvent(Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        if (e.UserId != currentUser.UserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentUser.UserId);
        }
    }

    private async Task EnsureThatUserOwnsTheReview(Guid reviewId)
    {
        var ec = await eventRepository.GetEventReviewById(reviewId);
        if (ec.UserId != currentUser.UserId)
        {
            throw new UnauthorizedUserForReviewException(ec.Id, ec.UserId, currentUser.UserId);
        }
    }

    public async Task<EventDto> AddEvent(CreateEventRequestDto createEvent)
    {
        var eventEntity = createEvent.ToEntity(currentUser.UserId);
        var e = await eventRepository.AddEvent(eventEntity);
        
        return e.ToDto();
    }

    public async Task<EventDto> GetEventByEventId(Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        return e.ToDto();
    }

    public async Task<List<EventOverviewDto>> GetEventsByUserId(Guid loggedInUserId)
    {
        var events = await eventRepository.GetEventsByUserId(loggedInUserId);
        return events.Select(x => x.ToEventOverviewDto(currentUser.Username))
            .ToList();
    }

    public async Task<List<EventDto>> GetAllEvents()
    {
        return await eventRepository.GetAllEvents();
    }

    public async Task<List<EventOverviewDto>> GetRecentEvents(PaginationDto paginationDto)
    {
        var filterSpecification = new EventsFilterSpecificationBuilder();
        var filters = filterSpecification.FilterByActive(true).Build();

        var events = await eventRepository.GetPaginatedAndFilteredEvents(filters, paginationDto);
        if (events is not null)
        {
            var userIds = events.Select(x => x.UserId).Distinct().ToList();
            var users = await userApiService.GetUsersByIds(userIds);

            var eventDtos = events.Select(x => x.ToEventOverviewDto(users)).ToList();
            return eventDtos;
        }
        return Enumerable.Empty<EventOverviewDto>().ToList();
    }

    public async Task<EventDto> UpdateEvent(Guid eventId, UpdateEventRequestDto updateReq)
    {
        await EnsureThatUserOwnsTheEvent(eventId);
        return await eventRepository.UpdateEvent(eventId, @event =>
        {
            @event.Name = updateReq.Name;
            @event.Description = updateReq.Description;
            @event.MaxAllowedParticipants = updateReq.MaxAllowedParticipants;
            @event.MinAllowedAge = updateReq.MinAllowedAge;
            @event.MaxAllowedAge = updateReq.MaxAllowedAge;
            @event.StartDate = updateReq.StartDate;
            @event.ReservationEndDate = updateReq.ReservationEndDate;
            @event.ImageThumbnail = updateReq.ImageThumbnailUrl;
        });
    }

    public async Task<EventDto> DeleteEvent(Guid eventId)
    {
        await EnsureThatUserOwnsTheEvent(eventId);

        var e = await eventRepository.DeleteEvent(eventId);
        await eventRepository.RemoveAllImagesFromEvent(eventId);

        return e.ToDto();
    }

    public async Task<EventDto> MakeEventPrivate(Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        if (e.UserId != currentUser.UserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentUser.UserId);
        }

        if (!e.IsPublic)
        {
            return e.ToDto();
        }

        var participants = await eventRepository.GetEventParticipants(eventId);

        foreach (var participant in participants)
        {
            var paymentIntent = await paymentService.CreatePaymentIntent(
                amount: 200,
                metadata: new Dictionary<string, string>
                {
                    { "EventId", e.EventId.ToString() },
                    { "ParticipantsUserId", participant.UserId.ToString() }
                }
            );

            participant.PaymentIntentId = paymentIntent.Id;
        }

        return await eventRepository.UpdateEvent(eventId, @event => @event.IsPublic = false);
    }

    public async Task<EventDto> MakeEventPublic(Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        if (e.UserId != currentUser.UserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentUser.UserId);
        }

        if (e.IsPublic)
        {
            return e.ToDto();
        }

        return await eventRepository.UpdateEvent(eventId, @event => @event.IsPublic = false);
    }

    // TODO Check if participants have already paid
    public async Task<EventDto> CancelEvent(Guid eventId)
    {
        await EnsureThatUserOwnsTheEvent(eventId);
        return await eventRepository.UpdateEvent(eventId, @event => @event.IsActive = false);
    }

    public async Task<EventDto> SignUpForEvent(Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        /* var userAge = CalculateAge(userBirthday);
        if (userAge < e.MinAllowedAge || userAge > e.MaxAllowedAge)
        {
            throw new UserIsNotTheRightAgeException(currentUser.UserId, e.MinAllowedAge, e.MaxAllowedAge);
        }
        */
        await eventRepository.AddEventParticipant(eventId, currentUser.UserId);
        return e.ToDto();
    }

    public async Task<EventDto> WithdrawFromEvent(Guid eventId)
    {
        if (!await eventRepository.IsUserParticipant(eventId, currentUser.UserId))
        {
            throw new UserIsNotSignedUpForEventException(currentUser.UserId, eventId);
        }

        var e = await eventRepository.RemoveEventParticipant(eventId, currentUser.UserId);
        return e.ToDto();
    }

    // TODO should everybody be able to see who's signed up for an event?
    public async Task<List<EventParticipantDto>> ViewEventParticipants(Guid eventId)
    {
        var participants = await eventRepository.GetEventParticipants(eventId);
        return participants.Select(ep => ep.ToDto()).ToList();
    }

    public async Task<Guid> CreateReview(Guid eventId, CreateEventReviewRequestDto createReq)
    {
        Guid userId = currentUser.UserId;
        var e = await eventRepository.GetEventById(eventId);
        var participants = await eventRepository.GetEventParticipants(eventId);

        if (!participants.Any(id => id.UserId == userId) || e.IsActive)
        {
            throw new CannotReviewException(eventId);
        }

        var reviewEntity = createReq.ToEntity(eventId, userId);
        await eventRepository.AddEventReview(reviewEntity);
        return reviewEntity.Id;
    }

    public async Task DeleteReview(Guid reviewId)
    {
        await EnsureThatUserOwnsTheReview(reviewId);
        var review = await eventRepository.DeleteEventReview(reviewId);
    }

    public async Task<Guid> EditReview(Guid reviewId, UpdateEventReviewRequestDto updateReq)
    {
        await EnsureThatUserOwnsTheReview(reviewId);
        var updatedReview = await eventRepository.UpdateEventReview(reviewId, review =>
        {
            review.ReviewStars = updateReq.Stars;
            review.ReviewComment = updateReq.Comment;
        });
        return reviewId;
    }


    public async Task<List<EventReviewDto>> GetAllReviewsForAnEvent(Guid eventId)
    {
        var eventEntity = await eventRepository.GetEventById(eventId);
        if (eventEntity.EventReviews is not null)
        {
            var userIds = eventEntity.EventReviews.Select(x => x.UserId).Distinct().ToList();
            var users = await userApiService.GetUsersByIds(userIds);

            return eventEntity.EventReviews.Select(x => x.ToDto(users)).ToList();
        }
        return Enumerable.Empty<EventReviewDto>().ToList();
    }

    public async Task<EventImageDto> AddEventImage(Guid eventId, AddEventImageRequestDto createReq)
    {
        await EnsureThatUserOwnsTheEvent(currentUser.UserId);

        var image = createReq.ToEntity(eventId);
        //await eventRepository.AddImageToEvent(eventId, image);
        return image.ToDto();
    }

    public async Task<EventImageDto> RemoveEventImage(Guid imageId)
    {
        await EnsureThatUserOwnsTheEvent(currentUser.UserId);
        var image = await eventRepository.RemoveImageFromEvent(imageId);
        return image.ToDto();
    }

    public async Task<EventFoodDetailsDto> GetEventFoodDetails(Guid eventId)
    {
        throw new NotImplementedException();
        // Make sure user is logged in? or is it enough to have it on event
        // var fd = await eventRepository.GetEventFoodDetails(eventId);
        // return fd.ToDto();
    }

    public async Task<EventFoodDetailsDto> EditFoodDetailsForEvent(Guid eventId, UpdateEventFoodRequest upReq)
    {
        throw new NotImplementedException();
        // await EnsureThatUserOwnsTheEvent(eventId);
        // var fd = await eventRepository.UpdateEventFoodDetails(eventId, food =>
        // {
        //     food.Name = upReq.Name;
        //     food.Ingredients = upReq.Ingredients;
        //     food.AdditionalFoodItems = upReq.AdditionalFoodItems;
        // });
        //
        // return fd.ToDto();
    }

    public async Task<EventFoodDetailsDto> DeleteFoodDetailsForEvent(Guid eventId)
    {
        throw new NotImplementedException();
        // var fd = await eventRepository.RemoveEventFoodDetails(eventId);
        // return fd.ToDto();
    }

    public async Task HandlePaymentSuccess(string paymentIntentId)
    {
        var participant = await eventRepository.GetEventParticipantByPaymentIntentId(paymentIntentId);
        await eventRepository.UpdateEventParticipant(participant, ep =>
        {
            ep.PaymentStatus = PaymentStatus.Paid;
        });
    }

    private static int CalculateAge(DateOnly birthday)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - birthday.Year;

        if (birthday > today.AddYears(-age))
        {
            age--;
        }
        return age;
    }

    public async Task<List<EventOverviewDto>> GetEventsWhereUserIsParticipant(Guid userId)
    {
        var events = await eventRepository.GetEventsByUserAsParticipant(userId);
        if (events is not null)
        {
            return events.Select(x => x.ToEventOverviewDto(currentUser.Username))
            .ToList();
        }
        return Enumerable.Empty<EventOverviewDto>().ToList();
    }

    public async Task<List<EventReviewDto>> GetEventReviewsByUserId(Guid userId)
    {
        var eventReviews = await eventRepository.GetEventReviewsByUserId(userId);
        if (eventReviews is not null)
        {
            var userIds = eventReviews.Select(x => x.UserId).Distinct().ToList();
            var users = await userApiService.GetUsersByIds(userIds);

            return eventReviews.Select(x => x.ToDto(users)).ToList();
        }
        return Enumerable.Empty<EventReviewDto>().ToList();
    }

    public async Task<List<EventOverviewDto>> GetFilteredAndPaginatedEvents(SearchEventsRequestDto searchEventsRequestDto, PaginationDto paginationDto)
    {
        var filterSpecification = new EventsFilterSpecificationBuilder();
        var filters = filterSpecification
            .FilterByActive(true)
            .FilterByPublic(searchEventsRequestDto.IsPublic)
            .FilterByName(searchEventsRequestDto.LocationOrEventName)
            .FilterByMinAndMaxAge(searchEventsRequestDto.MinAge, searchEventsRequestDto.MaxAge)
            .Build();

        var events = await eventRepository.GetPaginatedAndFilteredEvents(filters, paginationDto);
        if (events is not null)
        {
            var userIds = events.Select(x => x.UserId).Distinct().ToList();
            var users = await userApiService.GetUsersByIds(userIds);

            var eventDtos = events.Select(x => x.ToEventOverviewDto(users)).ToList();
            return eventDtos;
        }
        return Enumerable.Empty<EventOverviewDto>().ToList();
    }
}