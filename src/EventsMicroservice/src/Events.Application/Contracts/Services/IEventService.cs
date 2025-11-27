using Events.Application.Dtos;
using Events.Application.Dtos.Common;
using Events.Application.Dtos.Requests;

namespace Events.Application.Contracts.Services;

public interface IEventService
{
    // Basic CRUD
    Task<EventDto> AddEvent(CreateEventRequestDto createEvent);
    Task<EventDto> GetEventByEventId(Guid eventId);
    Task<List<EventOverviewDto>> GetEventsByUserId(Guid userId);
    Task<List<EventDto>> GetAllEvents();
    Task<List<EventOverviewDto>> GetRecentEvents(PaginationDto paginationDto);
    Task<EventDto> UpdateEvent(Guid eventId, UpdateEventRequestDto updateReq);
    Task<EventDto> DeleteEvent(Guid eventId);
    Task<List<EventOverviewDto>> GetEventsWhereUserIsParticipant(Guid userId);
    Task<List<EventReviewDto>> GetEventReviewsByUserId(Guid userId);
    Task<List<EventOverviewDto>> GetFilteredAndPaginatedEvents(SearchEventsRequestDto searchEventsRequestDto, PaginationDto paginationDto);

    // Event specific
    Task<EventDto> MakeEventPrivate(Guid eventId);
    Task<EventDto> MakeEventPublic(Guid eventId);
    Task<EventDto> CancelEvent(Guid eventId);

    // Event participation
    Task<EventDto> SignUpForEvent(Guid eventId);
    Task<EventDto> WithdrawFromEvent(Guid eventId);
    Task<List<EventParticipantDto>> ViewEventParticipants(Guid eventId);

    // Event Comments 
    Task<Guid> CreateReview(Guid eventId, CreateEventReviewRequestDto createReq);
    Task DeleteReview(Guid reviewId);
    Task<Guid> EditReview(Guid reviewId, UpdateEventReviewRequestDto updateReq);
    Task<List<EventReviewDto>> GetAllReviewsForAnEvent(Guid eventId);

    // EventImages
    Task<EventImageDto> AddEventImage(Guid eventId, AddEventImageRequestDto createReq);
    Task<EventImageDto> RemoveEventImage(Guid imageId);

    // Food Details 
    // Don't need one for add? since you should always create fooddetails when you add an event
    Task<EventFoodDetailsDto> GetEventFoodDetails(Guid eventId);
    Task<EventFoodDetailsDto> EditFoodDetailsForEvent(Guid eventId, UpdateEventFoodRequest upReq);
    Task<EventFoodDetailsDto> DeleteFoodDetailsForEvent(Guid eventId);

    // Stripe 
    Task HandlePaymentSuccess(string paymentIntentId);
}