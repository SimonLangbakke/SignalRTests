namespace ChatMicroservice.Application.Exceptions;

public class UnauthorizedChatAccessException(Guid chatRoomId, Guid userId)
    : Exception($"User {userId} is not authorized to access chat room {chatRoomId}.")
{ }