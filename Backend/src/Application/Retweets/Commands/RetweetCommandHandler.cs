namespace Application.Retweets.Commands;

public class RetweetCommandHandler : IRequestHandler<RetweetCommand, Result<RetweetResponse>>
{
    private readonly IRetweetService _retweetService;
    private readonly ILogger<RetweetCommandHandler> _logger;

    public RetweetCommandHandler(IRetweetService retweetService,
        ILogger<RetweetCommandHandler> logger)
    {
        _retweetService = retweetService;
        _logger = logger;
    }

    public async Task<Result<RetweetResponse>> Handle(RetweetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var retweetResult = await _retweetService.RetweetAsync(request.TweetId!);

            if (retweetResult != null)
            {
                var response = new RetweetResponse
                {
                    IsRetweetedByCurrentUser = retweetResult.Value.IsRetweetedByCurrentUser,
                    Retweets = retweetResult.Value.Retweets
                };
                _logger.LogInformation(CommandMessages<RetweetCommand>.CommandExecutedSuccessfully);
                
                return Result<RetweetResponse>.Success(response);
            }
            _logger.LogError(CommandMessages<RetweetCommand>.FailedToExecuteCommand);

            return Result<RetweetResponse>.Failure(CommandMessages<RetweetCommand>.FailedToExecuteCommand);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<RetweetCommand>.FailedToExecuteCommandException, ex.Message);
            
            return Result<RetweetResponse>.Failure(CommandMessages<RetweetCommand>.FailedToExecuteCommandException);
        }
    }
}
