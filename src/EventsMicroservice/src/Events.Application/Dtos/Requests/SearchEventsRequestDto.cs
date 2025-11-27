using Microsoft.AspNetCore.Mvc;

namespace Events.Application.Dtos.Requests;

public class SearchEventsRequestDto
{
    [FromQuery(Name = "locationOrEventName")]
    public string? LocationOrEventName { get; set; }

    [FromQuery(Name = "minPrice")]
    public int? MinPrice { get; set; }

    [FromQuery(Name = "maxPrice")]
    public int? MaxPrice { get; set; }

    [FromQuery(Name = "fromDate")]
    public DateTime? FromDate { get; set; }

    [FromQuery(Name = "toDate")]
    public DateTime? ToDate { get; set; }

    [FromQuery(Name = "minAge")]
    public int? MinAge { get; set; }

    [FromQuery(Name = "maxAge")]
    public int? MaxAge { get; set; }

    [FromQuery(Name = "isPublic")]
    public bool? IsPublic { get; set; }
}
