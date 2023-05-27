namespace Application.Follows.Commands.FollowUser;

public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, Result<FollowResponse>>
{
    private readonly IFollowUserService _followUserService;
    private readonly ILogger<FollowUserCommandHandler> _logger;

    public FollowUserCommandHandler(IFollowUserService followUserService,
        ILogger<FollowUserCommandHandler> logger)
    {
        _followUserService = followUserService;
        _logger = logger;
    }

    public async Task<Result<FollowResponse>> Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var followResult = await _followUserService.FollowUserAsync(request.TargetUserId);

            if (followResult.HasValue)
            {
                var response = new FollowResponse
                {
                    IsFollowing = followResult.Value.IsFollowing,
                    Followers = followResult.Value.Followers
                };
                _logger.LogInformation(CommandMessages<FollowUserCommand>.CommandExecutedSuccessfully);
                
                return Result<FollowResponse>.Success(response);
            }

            _logger.LogError(CommandMessages<FollowUserCommand>.FailedToExecuteCommand);
            
            return Result<FollowResponse>.Failure(CommandMessages<FollowUserCommand>.FailedToExecuteCommand);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<FollowUserCommand>.FailedToExecuteCommandException, ex.Message);
            
            return Result<FollowResponse>.Failure(CommandMessages<FollowUserCommand>.FailedToExecuteCommandException);
        }
    }
}
