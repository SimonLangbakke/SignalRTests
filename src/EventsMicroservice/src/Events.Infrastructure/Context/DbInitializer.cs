using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Context;

public static class DbInitializer
{
    public static void Seed(EventsDbContext context)
    {
        context.Database.Migrate();
        
        if (!context.Events.Any())
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            
            var e1Id = Guid.NewGuid();
            var fd1 = new EventFoodDetails
            {
                DietaryStyles = [],
                Allergens = [],
                Name = "Pizza",
                Ingredients = "Flour, water, meat, cheese",
                AdditionalFoodItems = "Monster Energy Nitro"
            };
            
            var e1 = new Event
            {
                EventId = e1Id,
                UserId = userId,
                Name = "Cozy Candlelit Dinner",
                Description = "Freshly made",
                MaxAllowedParticipants = 5,
                MinAllowedAge = 18,
                MaxAllowedAge = 25,
                StartDate = new DateTime(2025, 12, 24, 18, 0, 0, DateTimeKind.Utc),
                ReservationEndDate = new DateTime(2025, 12, 24, 20, 0, 0, DateTimeKind.Utc),
                ImageThumbnail = "",
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                IsPublic = true,
                EventFoodDetails = fd1
                
            };   
            

            // Event 2
            var e2Id = Guid.NewGuid();
            var fd2 = new EventFoodDetails
            {
                DietaryStyles = [],
                Allergens = [],
                Name = "Sushi Platter",
                Ingredients = "Rice, fish, seaweed, wasabi",
                AdditionalFoodItems = "Green Tea"
            };

            var e2 = new Event
            {
                EventId = e2Id,
                UserId = userId,
                Name = "Japanese Sushi Night",
                Description = "Fresh sushi made to order",
                MaxAllowedParticipants = 10,
                MinAllowedAge = 21,
                MaxAllowedAge = 35,
                StartDate = new DateTime(2026, 1, 15, 19, 30, 0, DateTimeKind.Utc),
                ReservationEndDate = new DateTime(2026, 1, 15, 22, 0, 0, DateTimeKind.Utc),
                ImageThumbnail = "",
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                IsPublic = true,
                EventFoodDetails = fd2
            };

            // Event 3
            var e3Id = Guid.NewGuid();
            var fd3 = new EventFoodDetails
            {
                DietaryStyles = [],
                Allergens = [],
                Name = "BBQ Feast",
                Ingredients = "Beef, chicken, sausages, veggies",
                AdditionalFoodItems = "Lemonade, Beer"
            };

            var e3 = new Event
            {
                EventId = e3Id,
                UserId = userId,
                Name = "Summer BBQ Bash",
                Description = "Outdoor grilling and fun",
                MaxAllowedParticipants = 20,
                MinAllowedAge = 18,
                MaxAllowedAge = 40,
                StartDate = new DateTime(2026, 6, 20, 16, 0, 0, DateTimeKind.Utc),
                ReservationEndDate = new DateTime(2026, 6, 20, 23, 0, 0, DateTimeKind.Utc),
                ImageThumbnail = "",
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                IsPublic = true,
                EventFoodDetails = fd3
            };

            // Add events and food details to context
            context.Events.AddRange(e1, e2, e3);
            context.EventFoodDetails.AddRange(fd1, fd2, fd3);
            context.SaveChanges();
        }
    }
}