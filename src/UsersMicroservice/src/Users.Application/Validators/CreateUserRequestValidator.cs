using FluentValidation;
using Users.Application.Dtos.Requests;

namespace Users.Application.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        RuleFor(x => x.Age)
            .NotEmpty().WithMessage("Age is required.")
            .Must(age => age >= 18).WithMessage("User must be at least 18 years old.");
    }
}
