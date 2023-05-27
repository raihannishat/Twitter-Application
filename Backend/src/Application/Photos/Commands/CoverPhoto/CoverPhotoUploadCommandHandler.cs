namespace Application.Photos.Commands.CoverPhoto;

public class CoverPhotoUploadCommandHandler : IRequestHandler<CoverPhotoUploadCommand, Result<CoverPhotoUploadResponse>>
{
    private readonly IPhotoService _photoService;
    private readonly ILogger<CoverPhotoUploadCommandHandler> _logger;

    public CoverPhotoUploadCommandHandler(IPhotoService photoService,
        ILogger<CoverPhotoUploadCommandHandler> logger)
    {
        _photoService = photoService;
        _logger = logger;
    }

    public async Task<Result<CoverPhotoUploadResponse>> Handle(CoverPhotoUploadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var coverPhotoResult = await _photoService.UploadCoverPhotoAsync(request.File!);

            if (coverPhotoResult != null)
            {
                var response = new CoverPhotoUploadResponse
                {
                    Image = coverPhotoResult
                };
                _logger.LogInformation(CommandMessages<CoverPhotoUploadCommand>.CommandExecutedSuccessfully);

                return Result<CoverPhotoUploadResponse>.Success(response);
            }
            _logger.LogError(CommandMessages<CoverPhotoUploadCommand>.FailedToExecuteCommand);

            return Result<CoverPhotoUploadResponse>.Failure(CommandMessages<CoverPhotoUploadCommand>.FailedToExecuteCommand);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<CoverPhotoUploadCommand>.FailedToExecuteCommandException,ex.Message);

            return Result<CoverPhotoUploadResponse>.Failure(CommandMessages<CoverPhotoUploadCommand>.FailedToExecuteCommandException);
        }
    }
}
