namespace Chat.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IChatRoomRepository _chatRoomRepository;
    private readonly IUserIntegrationService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IValidator<SendMessageRequestDto> _validator;

    public MessageService(
        IMessageRepository messageRepository,
        IChatRoomRepository chatRoomRepository,
        IUserIntegrationService userService,
        IHttpContextAccessor httpContextAccessor,
        IValidator<SendMessageRequestDto> validator)
    {
        _messageRepository = messageRepository;
        _chatRoomRepository = chatRoomRepository;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
        _validator = validator;
    }

    public async Task<MessageDto> SendMessageAsync(SendMessageRequestDto dto)
    {
        // Validate using FluentValidation like in your UserService
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new RequestValidationException(
                "Invalid message request",
                validationResult.Errors.ToDictionary(
                    e => e.PropertyName,
                    e => e.ErrorMessage));
        }

        // Get user ID from JWT token (same pattern as your UserController)
        var userId = GetUserIdFromToken();

        // Verify user is a member of the chat room
        var membership = await _chatRoomRepository.GetMembershipAsync(userId, dto.ChatRoomId);
        if (membership == null)
        {
            throw new UnauthorizedChatAccessException("User is not a member of this chat room");
        }

        // Create and save the message
        var message = new Message
        {
            Id = Guid.NewGuid(),
            ChatRoomId = dto.ChatRoomId,
            SenderId = userId,
            Content = dto.Content,
            SentAt = DateTime.UtcNow
        };

        await _messageRepository.AddMessageAsync(message);

        // Fetch sender info for the response
        var senderInfo = await _userService.GetUserInfoAsync(userId);

        return new MessageDto(
            message.Id,
            message.SenderId,
            senderInfo?.Name ?? "Unknown",
            message.Content,
            message.SentAt,
            message.IsPinned,
            membership.IsHost  // Can pin if host
        );
    }

    public async Task<MessageDto> PinMessageAsync(Guid messageId)
    {
        var userId = GetUserIdFromToken();
        var message = await _messageRepository.GetMessageAsync(messageId);

        if (message == null)
            throw new MessageNotFoundException(messageId);

        // Check if user is the host
        var membership = await _chatRoomRepository.GetMembershipAsync(userId, message.ChatRoomId);
        if (membership?.IsHost != true)
        {
            throw new UnauthorizedChatAccessException("Only hosts can pin messages");
        }

        message.IsPinned = true;
        message.PinnedAt = DateTime.UtcNow;
        message.PinnedByUserId = userId;

        await _messageRepository.UpdateMessageAsync(message);

        var senderInfo = await _userService.GetUserInfoAsync(message.SenderId);

        return new MessageDto(
            message.Id,
            message.SenderId,
            senderInfo?.Name ?? "Unknown",
            message.Content,
            message.SentAt,
            true,
            true
        );
    }

    private Guid GetUserIdFromToken()
    {
        // Extract from JWT "sub" claim
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var userId))
        {
            throw new UnauthorizedException("Invalid or missing user ID in token");
        }
        return userId;
    }
}