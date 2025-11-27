-- Mock Data for FoodClub Events

-- Clear existing mock data (optional - remove if you want to keep existing events)
DELETE FROM "EventFoodDetails" WHERE "EventId" IN (
    '11111111-1111-1111-1111-111111111111',
    '22222222-2222-2222-2222-222222222222',
    '33333333-3333-3333-3333-333333333333'
);
DELETE FROM "Events" WHERE "EventId" IN (
    '11111111-1111-1111-1111-111111111111',
    '22222222-2222-2222-2222-222222222222',
    '33333333-3333-3333-3333-333333333333'
);

-- Event 1: Italian Pasta Night
INSERT INTO "Events" (
    "EventId", "UserId", "Name", "Description", "GuestNotes", 
    "MaxAllowedParticipants", "PricePerSeat", "MinAllowedAge", "MaxAllowedAge", 
    "StartDate", "EndDate", "ReservationEndDate", "ImageThumbnail", 
    "CreatedDate", "IsActive", "IsPublic", 
    "EventAddress_City", "EventAddress_PostalCode", "EventAddress_Region", "EventAddress_StreetAddress"
) VALUES (
    '11111111-1111-1111-1111-111111111111', '99999999-9999-9999-9999-999999999999', 
    'Authentic Italian Pasta Night', 
    'Join us for a lovely evening of homemade pasta and good company. We will be making fresh tagliatelle with a slow-cooked ragu.', 
    'Please bring your own wine if you like.', 
    6, 150.00, 18, 99, 
    NOW() + INTERVAL '2 days', NOW() + INTERVAL '2 days 3 hours', NOW() + INTERVAL '1 day', 
    'https://images.unsplash.com/photo-1551183053-bf91b1d511a3?w=800', 
    NOW(), true, true, 
    'Copenhagen', '2200', 'Hovedstaden', 'Nørrebrogade 123'
);

INSERT INTO "EventFoodDetails" (
    "Id", "EventId", "Name", "Ingredients", "AdditionalFoodItems", "DietaryStyles", "Allergens"
) VALUES (
    '10000000-0000-0000-0000-000000000001', 
    '11111111-1111-1111-1111-111111111111', 
    'Homemade Tagliatelle al Ragu', 
    'Flour, Eggs, Beef, Tomatoes, Onions, Carrots, Celery', 
    'Tiramisu for dessert', 
    ARRAY['Italian', 'Comfort Food'], 
    ARRAY['Gluten', 'Eggs']
);

-- Event 2: Spicy Taco Fiesta
INSERT INTO "Events" (
    "EventId", "UserId", "Name", "Description", "GuestNotes", 
    "MaxAllowedParticipants", "PricePerSeat", "MinAllowedAge", "MaxAllowedAge", 
    "StartDate", "EndDate", "ReservationEndDate", "ImageThumbnail", 
    "CreatedDate", "IsActive", "IsPublic", 
    "EventAddress_City", "EventAddress_PostalCode", "EventAddress_Region", "EventAddress_StreetAddress"
) VALUES (
    '22222222-2222-2222-2222-222222222222', '99999999-9999-9999-9999-999999999999', 
    'Spicy Taco Fiesta', 
    'Tacos, salsa, and guacamole! Best Mexican food in town. We will have both beef and vegetarian options.', 
    'Come hungry!', 
    8, 200.00, 21, 50, 
    NOW() + INTERVAL '5 days', NOW() + INTERVAL '5 days 4 hours', NOW() + INTERVAL '3 days', 
    'https://images.unsplash.com/photo-1565299585323-38d6b0865b47?w=800', 
    NOW(), true, true, 
    'Aarhus', '8000', 'Midtjylland', 'Vestergade 45'
);

INSERT INTO "EventFoodDetails" (
    "Id", "EventId", "Name", "Ingredients", "AdditionalFoodItems", "DietaryStyles", "Allergens"
) VALUES (
    '20000000-0000-0000-0000-000000000002', 
    '22222222-2222-2222-2222-222222222222', 
    'Street Tacos', 
    'Corn tortillas, Beef, Beans, Avocado, Cilantro, Lime', 
    'Churros', 
    ARRAY['Mexican', 'Spicy'], 
    ARRAY['Corn']
);

-- Event 3: Traditional Danish Smørrebrød
INSERT INTO "Events" (
    "EventId", "UserId", "Name", "Description", "GuestNotes", 
    "MaxAllowedParticipants", "PricePerSeat", "MinAllowedAge", "MaxAllowedAge", 
    "StartDate", "EndDate", "ReservationEndDate", "ImageThumbnail", 
    "CreatedDate", "IsActive", "IsPublic", 
    "EventAddress_City", "EventAddress_PostalCode", "EventAddress_Region", "EventAddress_StreetAddress"
) VALUES (
    '33333333-3333-3333-3333-333333333333', '99999999-9999-9999-9999-999999999999', 
    'Traditional Danish Smørrebrød', 
    'Experience true Danish hygge with classic open-faced sandwiches. We will have herring, roast beef, and egg & shrimp.', 
    'Snaps will be served!', 
    4, 250.00, 25, 99, 
    NOW() + INTERVAL '10 days', NOW() + INTERVAL '10 days 3 hours', NOW() + INTERVAL '7 days', 
    'https://images.unsplash.com/photo-1600891964599-f61ba0e24092?w=800', 
    NOW(), true, true, 
    'Odense', '5000', 'Syddanmark', 'Overgade 10'
);

INSERT INTO "EventFoodDetails" (
    "Id", "EventId", "Name", "Ingredients", "AdditionalFoodItems", "DietaryStyles", "Allergens"
) VALUES (
    '30000000-0000-0000-0000-000000000003', 
    '33333333-3333-3333-3333-333333333333', 
    'Classic Smørrebrød Selection', 
    'Rye bread, Butter, Herring, Roast Beef, Eggs, Shrimp, Remoulade', 
    'Coffee and cake', 
    ARRAY['Danish', 'Traditional'], 
    ARRAY['Gluten', 'Fish', 'Eggs', 'Milk']
);
