namespace Application.Photos.Commands.CoverPhoto;

public class CoverPhotoUploadCommand : IRequest<Result<CoverPhotoUploadResponse>>
{
    public IFormFile? File { get; set; }
}
