namespace Application.Tweets.Queries.GetTweetById;

public class GetTweetByIdQueryHandler : IRequestHandler<GetTweetByIdQuery, Result<TweetViewModel>>
{
    private readonly ITweetService _tweetService;
    private readonly ILogger<GetTweetByIdQueryHandler> _logger;

    public GetTweetByIdQueryHandler(ITweetService tweetService,
        ILogger<GetTweetByIdQueryHandler> logger)
    {
        _tweetService = tweetService;
        _logger = logger;
    }

    public async Task<Result<TweetViewModel>> Handle(GetTweetByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _tweetService.GetTweetByIdAsync(request.TweetId, request.TweetOwnerId);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetTweetByIdQuery>.QueryExecutedSuccessfully);

                return Result<TweetViewModel>.Success(response);
            }

            _logger.LogError(QueryMessages<GetTweetByIdQuery>.FailedToExecuteQuery);

            return Result<TweetViewModel>.Failure(QueryMessages<GetTweetByIdQuery>.FailedToExecuteQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetTweetByIdQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<TweetViewModel>.Failure(QueryMessages<GetTweetByIdQuery>.FailedToExecuteQueryException);
        }
    }
}
