namespace Application.Photos.Commands.ProfilePhoto;

public class ProfilePhotoUploadCommandHandler : IRequestHandler<ProfilePhotoUploadCommand, Result<ProfilePhotoUploadResponse>>
{
    private readonly IPhotoService _photoService;
    private readonly ILogger<ProfilePhotoUploadCommandHandler> _logger;

    public ProfilePhotoUploadCommandHandler(IPhotoService photoService,
        ILogger<ProfilePhotoUploadCommandHandler> logger)
    {
        _photoService = photoService;
        _logger = logger;
    }

    public async Task<Result<ProfilePhotoUploadResponse>> Handle(ProfilePhotoUploadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var profilePhotoResult = await _photoService.UploadProfilePhotoAsync(request.File!);

            if (profilePhotoResult != null)
            {
                var response = new ProfilePhotoUploadResponse
                {
                    Image = await _photoService.UploadProfilePhotoAsync(request.File!)
                };

                _logger.LogInformation(CommandMessages<ProfilePhotoUploadCommand>.CommandExecutedSuccessfully);

                return Result<ProfilePhotoUploadResponse>.Success(response);
            }

            _logger.LogError(CommandMessages<ProfilePhotoUploadCommand>.FailedToExecuteCommand);

            return Result<ProfilePhotoUploadResponse>.Failure(CommandMessages<ProfilePhotoUploadCommand>.FailedToExecuteCommand);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<ProfilePhotoUploadCommand>.FailedToExecuteCommandException, ex.Message);

            return Result<ProfilePhotoUploadResponse>.Failure(CommandMessages<ProfilePhotoUploadCommand>.FailedToExecuteCommandException);
        }
    }
}
