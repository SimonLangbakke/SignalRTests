namespace Events.Application.Exceptions;

public class ImageNotFoundException : EventApplicationException
{
    public ImageNotFoundException(Guid imageId)
        : base($"No image with id {imageId} was found.") { }
}