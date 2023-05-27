namespace Application.Tweets.Commands.DeleteTweet;

public class DeleteTweetCommandHandler : IRequestHandler<DeleteTweetCommand, Result<Unit>>
{
    private readonly ITweetService _tweetService;
    private readonly ILogger<DeleteTweetCommandHandler> _logger;

    public DeleteTweetCommandHandler(ITweetService tweetService,
        ILogger<DeleteTweetCommandHandler> logger)
    {
        _tweetService = tweetService;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteTweetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _tweetService.DeleteTweetAsync(request.Id);

            if (result != null && result.Equals(true))
            {
                _logger.LogInformation(CommandMessages<DeleteTweetCommand>.CommandExecutedSuccessfully);

                return Result<Unit>.Success(Unit.Value);
            }

            _logger.LogInformation(CommandMessages<DeleteTweetCommand>.FailedToExecuteCommand);

            return Result<Unit>.Failure(CommandMessages<DeleteTweetCommand>.FailedToExecuteCommand);

        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<DeleteTweetCommand>.FailedToExecuteCommandException, ex.Message);

            return Result<Unit>.Failure(CommandMessages<DeleteTweetCommand>.FailedToExecuteCommandException);
        }
    }
}
