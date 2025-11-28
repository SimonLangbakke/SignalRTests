namespace ChatMicroservice.Application.Exceptions;

public class MessageNotFoundException(Guid messageId)
    : Exception($"Message with ID: {messageId} not found.")
{ }