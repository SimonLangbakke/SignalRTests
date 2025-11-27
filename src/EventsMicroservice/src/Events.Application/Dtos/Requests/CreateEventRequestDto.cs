using Events.Domain.Entities;

namespace Events.Application.Dtos.Requests;

/// <summary>
/// Defines the contract for creating a new event with scheduling, capacity, address and media information.
/// </summary>
/// <param name="Name">Human friendly title shown to attendees and in listings.</param>
/// <param name="Description">Detailed explanation of what the event offers (required, max 150 chars).</param>
/// <param name="MaxAllowedParticipants">Maximum number of participants that can register.</param>
/// <param name="PricePerSeat">Ticket price per seat, used for paid events.</param>
/// <param name="MinAllowedAge">Youngest age allowed to join the event.</param>
/// <param name="MaxAllowedAge">Oldest age allowed to join the event.</param>
/// <param name="StartDate">Date and time when the event starts.</param>
/// <param name="EndDate">Optional date and time when the event finishes.</param>
/// <param name="ReservationEndDate">Cut-off date/time for accepting new reservations. Usually it should be on the same day.</param>
/// <param name="ImageThumbnail">Cover image shown in event previews (required, max 150 chars).</param>
/// <param name="IsPublic">Indicates whether the event is visible to everyone or invite-only.</param>
/// <param name="EventFoodDetails">Menu and dietary information associated with the event.</param>
/// <param name="Images">Additional gallery images that accompany the event listing.</param>
/// <param name="StreetAddress">Primary address line where the event takes place (required, max 256 chars).</param>
/// <param name="PostalCode">Postal or ZIP code for the venue (required, max 32 chars).</param>
/// <param name="City">City in which the event is hosted (required, max 128 chars).</param>
/// <param name="Region">State, province or broader region for the address (required, max 128 chars).</param>
public record CreateEventRequestDto(
    string Name,
    string Description,
    int MaxAllowedParticipants,
    double PricePerSeat,
    int MinAllowedAge,
    int MaxAllowedAge,
    DateTime StartDate,
    DateTime? EndDate,
    DateTime ReservationEndDate,
    string ImageThumbnail,
    bool IsPublic,
    EventFoodDetails EventFoodDetails,
    IEnumerable<EventImage> Images,
    string StreetAddress,
    string PostalCode,
    string City,
    string Region
);