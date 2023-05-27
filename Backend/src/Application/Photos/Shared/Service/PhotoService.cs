namespace Application.Photos.Shared.Service;

public class PhotoService : IPhotoService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IPhotoAccessor _photoAccessor;

    public PhotoService(ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IPhotoAccessor photoAccessor)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _photoAccessor = photoAccessor;
    }

    public async Task<string> UploadCoverPhotoAsync(IFormFile photo)
    {
        var currentUserId = _currentUserService.UserId;

        var user = await _userRepository.FindByIdAsync(currentUserId);

        if (user == null)
        {
            return null!;
        }

        var photoUploadResult = await _photoAccessor.AddCoverPhotoAsync(photo);

        if (photoUploadResult == null)
        {
            return null!;
        }

        user.CoverImage = photoUploadResult.Url;

        await _userRepository.ReplaceOneAsync(user);

        return user.CoverImage;
    }

    public async Task<string> UploadProfilePhotoAsync(IFormFile photo)
    {
        var currentUserId = _currentUserService.UserId;

        var user = await _userRepository.FindByIdAsync(currentUserId);

        if (user == null)
        {
            return null!;
        }

        var photoUploadResult = await _photoAccessor.AddProfilePhotoAsync(photo);

        if (photoUploadResult == null)
        {
            return null!;
        }

        user.Image = photoUploadResult.Url;

        await _userRepository.ReplaceOneAsync(user);

        return user.Image;
    }
}
