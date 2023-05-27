namespace Application.Photos.Shared.Service;

public interface IPhotoService
{
    Task<string> UploadCoverPhotoAsync(IFormFile photo);
    Task<string> UploadProfilePhotoAsync(IFormFile photo);
}
