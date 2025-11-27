using Events.Application.Dtos.Requests;
using FluentValidation;

namespace Events.Application.Validators;

public class CreateEventRequestDtoValidator : AbstractValidator<CreateEventRequestDto>
{
    public CreateEventRequestDtoValidator()
    {

    }
}
