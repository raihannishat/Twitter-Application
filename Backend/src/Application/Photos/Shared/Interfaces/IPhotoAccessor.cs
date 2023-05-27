namespace Application.Photos.Shared.Interfaces;

public interface IPhotoAccessor
{
    Task<PhotoUploadResult> AddProfilePhotoAsync(IFormFile file);
    Task<PhotoUploadResult> AddCoverPhotoAsync(IFormFile file);
}
