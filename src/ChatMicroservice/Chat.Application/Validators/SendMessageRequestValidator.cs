using FluentValidation;
using Chat.Application.Dtos.Requests;

namespace Chat.Application.Validators;

public class SendMessageRequestValidator : AbstractValidator<SendMessageRequestDto>
{
    public SendMessageRequestValidator()
    {
        RuleFor(x => x.ChatRoomId)
            .NotEmpty().WithMessage("ChatRoomId is required.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Message content is required.")
            .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters.");
    }
}