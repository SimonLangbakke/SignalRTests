using Events.Application.Dtos;
using Events.Application.Dtos.Common;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;
using System.Linq.Expressions;

namespace Events.Application.Contracts.Repositories;

public interface IEventRepository
{
    Task<Event> GetEventById(Guid id);
    Task<List<EventDto>> GetAllEvents(PaginationDto? paginationDto = null);
    Task<List<Event>> GetPaginatedAndFilteredEvents(List<Expression<Func<Event, bool>>> eventFilters, PaginationDto? paginationDto = null);
    Task<Event> AddEvent(Event createEvent);
    Task<EventDto> UpdateEvent(Guid id, Action<Event> op);
    Task<Event> DeleteEvent(Guid id);
    Task<List<Event>> GetEventsByUserId(Guid userId);
    Task<List<Event>> GetEventsByUserAsParticipant(Guid userId);

    /* ===========================================================================
     * =============== P A R T I C I P A N T S ===================================
     * ===========================================================================
     */
    Task<Event> AddEventParticipant(Guid eventId, Guid userId);
    Task<Event> RemoveEventParticipant(Guid eventId, Guid userId);
    Task<List<EventParticipant>> GetEventParticipants(Guid eventId);
    Task<bool> IsUserParticipant(Guid eventId, Guid userId);
    Task<EventParticipant> GetEventParticipantByPaymentIntentId(string paymentIntentId);
    Task<EventParticipant> UpdateEventParticipant(EventParticipant participant, Action<EventParticipant> op);


    /* ===========================================================================
     * ======================= R E V I E W S =====================================
     * ===========================================================================
     */
    Task<EventReview> AddEventReview(EventReview review);
    Task<EventReview> UpdateEventReview(Guid commentId, Action<EventReview> op);
    Task<EventReview> DeleteEventReview(Guid commentId);
    Task<List<EventReview>> GetEventReviews(Guid eventId);
    Task<EventReview> GetEventReviewById(Guid reviewId);
    Task<List<EventReview>> GetEventReviewsByUserId(Guid userId);

    /*
     * ===========================================================================
     * =========================== I M A G E S ===================================
     * ===========================================================================
     */
    EventImage AddImageToEvent(Guid eventId, EventImage image);
    Task<EventImage> RemoveImageFromEvent(Guid imageId);
    Task<List<EventImage>> RemoveAllImagesFromEvent(Guid eventId);

    /*
     * ===========================================================================
     * ===================== F O O D - D E T A I L S =============================
     * ===========================================================================
     */

    // Task<EventFoodDetails> AddEventFoodDetails(Guid eventId, EventFoodDetails foodDetails);
    // Task<EventFoodDetails> UpdateEventFoodDetails(Guid eventId, Action<EventFoodDetails> op);
    // Task<EventFoodDetails> GetEventFoodDetails(Guid eventId);
    // Task<EventFoodDetails> RemoveEventFoodDetails(Guid eventId);

}
