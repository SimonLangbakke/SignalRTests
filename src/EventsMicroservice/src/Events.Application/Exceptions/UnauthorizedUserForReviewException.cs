namespace Events.Application.Exceptions;

public class UnauthorizedUserForReviewException : EventApplicationException
{
    public UnauthorizedUserForReviewException(Guid reviewId, Guid commentUserId, Guid currentUserId)
        : base($"User {currentUserId} does not have access to review {reviewId}, only {commentUserId} does") { }
    
}