namespace Application.Tweets.Queries.GetHashtagTweets;

public class GetHashtagTweetsQueryHandler : IRequestHandler<GetHashtagTweetsQuery, Result<List<TweetViewModel>>>
{
    private readonly IHashtagService _hashtagService;
    private readonly ILogger<GetHashtagTweetsQueryHandler> _logger;

    public GetHashtagTweetsQueryHandler(IHashtagService hashtagService,
        ILogger<GetHashtagTweetsQueryHandler> logger)
    {
        _hashtagService = hashtagService;
        _logger = logger;
    }

    public async Task<Result<List<TweetViewModel>>> Handle(GetHashtagTweetsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _hashtagService.GetHashtagTweetsAsync(request.PageQuery.Keyword!, request.PageQuery.PageNumber);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetHashtagTweetsQuery>.QueryExecutedSuccessfully);

                return Result<List<TweetViewModel>>.Success(response.ToList());
            }
            _logger.LogError(QueryMessages<GetHashtagTweetsQuery>.FailedToExecuteQuery);

            return Result<List<TweetViewModel>>.Failure(QueryMessages<GetHashtagTweetsQuery>.FailedToExecuteQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetHashtagTweetsQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<List<TweetViewModel>>.Failure(QueryMessages<GetHashtagTweetsQuery>.FailedToExecuteQueryException);
        }
    }
}
