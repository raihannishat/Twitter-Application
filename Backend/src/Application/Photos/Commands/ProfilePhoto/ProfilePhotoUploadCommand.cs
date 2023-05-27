namespace Application.Photos.Commands.ProfilePhoto;

public class ProfilePhotoUploadCommand : IRequest<Result<ProfilePhotoUploadResponse>>
{
    public IFormFile? File { get; set; }
}
