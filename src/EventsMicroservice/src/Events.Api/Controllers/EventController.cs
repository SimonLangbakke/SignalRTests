using Events.Application.Contracts.Services;
using Events.Application.Dtos.Common;
using Events.Application.Dtos.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Events.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/event")]
public class EventController(IEventService _eventService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentEvents()
    {
        return Ok(await _eventService.GetRecentEvents(new PaginationDto(0, 10)));
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> GetFilteredEvents(SearchEventsRequestDto searchEventsRequestDto)
    {
        return Ok(await _eventService.GetFilteredAndPaginatedEvents(searchEventsRequestDto, new PaginationDto(0, 10)));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var dto = await _eventService.GetEventByEventId(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPut("{eventId:guid}/participate")]
    public async Task<IActionResult> SignUpForEvent(Guid eventId)
        => Ok(await _eventService.SignUpForEvent(eventId));
    
    [HttpPut("{eventId:guid}/withdraw")]
    public async Task<IActionResult> WithdrawFromEvent(Guid eventId)
        => Ok(await _eventService.WithdrawFromEvent(eventId));
    
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateEventRequestDto req)
    {
        var created = await _eventService.AddEvent(req);
        return CreatedAtAction(nameof(GetById), new { id = created!.EventId }, created);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId)
        => Ok(await _eventService.GetEventsByUserId(userId));

    [HttpGet("user-as-participant/{userId:guid}")]
    public async Task<IActionResult> GetByUserParticipant(Guid userId)
        => Ok(await _eventService.GetEventsWhereUserIsParticipant(userId));

    [HttpGet("user-reviews-as-host/{userId:guid}")]
    public async Task<IActionResult> GetEventReviewsByUserId(Guid userId)
        => Ok(await _eventService.GetEventReviewsByUserId(userId));



    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _eventService.DeleteEvent(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }

    [AllowAnonymous]
    [HttpPost("stripe/webhhok")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var endpointSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                endpointSecret
            );

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as Stripe.PaymentIntent;
                var paymentIntentId = paymentIntent.Id;

                await _eventService.HandlePaymentSuccess(paymentIntentId);
            }

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest(e.Message);
        }
    }
}
