namespace ChatMicroservice.Application.Exceptions;
public class ChatRoomNotFoundException(Guid chatRoomId)
    : Exception($"Chat room with ID: {chatRoomId} not found.")
{ }