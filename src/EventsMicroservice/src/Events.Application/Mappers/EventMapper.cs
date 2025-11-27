using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Mappers;

public static class EventMapper
{
    public static EventDto ToDto(this Event e)
    {
        return new EventDto(
            e.EventId,
            e.UserId,
            e.Name,
            e.Description,
            e.MaxAllowedParticipants,
            e.PricePerSeat,
            e.MinAllowedAge,
            e.MaxAllowedAge,
            e.StartDate,
            e.EndDate,
            e.ReservationEndDate,
            e.ImageThumbnail,
            e.CreatedDate,
            e.IsActive,
            e.IsPublic,
            e.EventAddress,
            e.EventFoodDetails,
            e.EventParticipants,
            e.EventImages
        );
    }

    public static EventOverviewDto ToEventOverviewDto(this Event e, IEnumerable<UserDto> users)
    {
        string[] mockImageThumbnails =
        [
            "https://i0.wp.com/blog.themalamarket.com/wp-content/uploads/2024/06/Vegetarian-pulled-noodles-lead-more-sat.jpg?resize=1200%2C900&ssl=1",
            "https://img.freepik.com/free-photo/penne-pasta-tomato-sauce-with-chicken-tomatoes-wooden-table_2829-19744.jpg?semt=ais_hybrid&w=740&q=80",
            "https://cdn.foodfaithfitness.com/uploads/2025/02/a-crunchy_roll_sushi-feature-2.jpeg"
        ];

        return new EventOverviewDto(
            e.EventId,
            e.UserId,
            users?.FirstOrDefault(x => x.Id == e.UserId)?.Name ?? string.Empty,
            4.4,
            e.Name,
            e.Description,
            e.MaxAllowedParticipants,
            e.PricePerSeat,
            e.MinAllowedAge,
            e.MaxAllowedAge,
            e.StartDate.Date.ToString("dd/MM/yyyy"),
            e.StartDate.ToString("hh:mm"),
            e.ReservationEndDate,
            e.EventFoodDetails?.Ingredients?.Split(',') ?? Array.Empty<String>(),
            e.EventParticipants.Count,
            mockImageThumbnails[new Random().Next(0, 2)],
            e.CreatedDate,
            e.PricePerSeat,
            e.IsActive,
            e.IsPublic,
            e.EventAddress,
            e.EventFoodDetails,
            e.EventParticipants,
            e.EventImages
        );
    }

    public static EventOverviewDto ToEventOverviewDto(this Event e, string userName)
    {
        string[] mockImageThumbnails =
        [
            "https://i0.wp.com/blog.themalamarket.com/wp-content/uploads/2024/06/Vegetarian-pulled-noodles-lead-more-sat.jpg?resize=1200%2C900&ssl=1",
            "https://img.freepik.com/free-photo/penne-pasta-tomato-sauce-with-chicken-tomatoes-wooden-table_2829-19744.jpg?semt=ais_hybrid&w=740&q=80",
            "https://cdn.foodfaithfitness.com/uploads/2025/02/a-crunchy_roll_sushi-feature-2.jpeg"
        ];

        return new EventOverviewDto(
            e.EventId,
            e.UserId,
            userName,
            4.4,
            e.Name,
            e.Description,
            e.MaxAllowedParticipants,
            e.PricePerSeat,
            e.MinAllowedAge,
            e.MaxAllowedAge,
            e.StartDate.Date.ToString("dd/MM/yyyy"),
            e.StartDate.ToString("hh:mm"),
            e.ReservationEndDate,
            e.EventFoodDetails?.Ingredients?.Split(',') ?? Array.Empty<String>(),
            e.EventParticipants.Count,
            mockImageThumbnails[new Random().Next(0, 2)],
            e.CreatedDate,
            e.PricePerSeat,
            e.IsActive,
            e.IsPublic,
            e.EventAddress,
            e.EventFoodDetails,
            e.EventParticipants,
            e.EventImages
        );
    }

    public static Event ToEntity(this CreateEventRequestDto dto, Guid userId)
    {
        return new Event
        {
            UserId = userId,
            Name = dto.Name,
            Description = dto.Description,
            MaxAllowedParticipants = dto.MaxAllowedParticipants,
            PricePerSeat = dto.PricePerSeat,
            MinAllowedAge = dto.MinAllowedAge,
            MaxAllowedAge = dto.MaxAllowedAge,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            ReservationEndDate = dto.ReservationEndDate,
            ImageThumbnail = dto.ImageThumbnail,
            IsActive = true,
            IsPublic = dto.IsPublic,
            EventFoodDetails = dto.EventFoodDetails,
            EventParticipants = Enumerable.Empty<EventParticipant>().ToList(),
            EventImages = dto.Images.ToList(),
            EventAddress = new EventAddress
            {
                StreetAddress = dto.StreetAddress,
                PostalCode = dto.PostalCode,
                City = dto.City,
                Region = dto.Region
            }
        };
    }
}