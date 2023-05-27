namespace Application.Likes.Commands;

public class LikeCommandHandler : IRequestHandler<LikeCommand, Result<LikeResponse>>
{
    private readonly ILikeService _likeService;
    private readonly ILogger<LikeCommandHandler> _logger;

    public LikeCommandHandler(ILikeService likeService,
        ILogger<LikeCommandHandler> logger)
    {
        _likeService = likeService;
        _logger = logger;
    }

    public async Task<Result<LikeResponse>> Handle(LikeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var likeResult = await _likeService.LikeAsync(request.TweetId!);

            if (likeResult != null)
            {
                var response = new LikeResponse
                {
                    IsLikedByCurrentUser = likeResult!.Value.IsLikedByCurrentUser,
                    Likes = likeResult.Value.Likes
                };

                return Result<LikeResponse>.Success(response);
            }
            _logger.LogError(CommandMessages<LikeCommand>.FailedToExecuteCommand);

            return Result<LikeResponse>.Failure(CommandMessages<LikeCommand>.FailedToExecuteCommand);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<LikeCommand>.FailedToExecuteCommandException, ex.Message);
            
            return Result<LikeResponse>.Failure(CommandMessages<LikeCommand>.FailedToExecuteCommandException);
        }
    }
}
