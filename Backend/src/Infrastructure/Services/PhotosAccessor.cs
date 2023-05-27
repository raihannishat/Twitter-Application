using Application.Photos.Shared.Interfaces;
using Application.Photos.Shared.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class PhotosAccessor : IPhotoAccessor
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<PhotosAccessor> _logger;

    public PhotosAccessor(IOptions<CloudinarySettings> config, ILogger<PhotosAccessor> logger)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret);

        _cloudinary = new Cloudinary(account);
        _logger = logger;
    }

    public async Task<PhotoUploadResult> AddCoverPhotoAsync(IFormFile file)
    {
        if (file.Length > 0)
        {
            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(1500).Crop("fill")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                _logger.LogError(uploadResult.Error.Message);

                return null!;
            }

            return new PhotoUploadResult
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString()
            };
        }

        return null!;
    }

    public async Task<PhotoUploadResult> AddProfilePhotoAsync(IFormFile file)
    {
        if (file.Length > 0)
        {
            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                _logger.LogError(uploadResult.Error.Message);

                return null!;
            }

            return new PhotoUploadResult
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString()
            };
        }

        return null!;
    }
}
