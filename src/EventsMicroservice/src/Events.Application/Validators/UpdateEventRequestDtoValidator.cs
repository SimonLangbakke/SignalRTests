using Events.Application.Dtos.Requests;
using FluentValidation;

namespace Events.Application.Validators;

public class UpdateEventRequestDtoValidator : AbstractValidator<UpdateEventRequestDto>
{
    public UpdateEventRequestDtoValidator()
    {

    }
}
