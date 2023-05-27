namespace Application.Tweets.Queries.GetTrendingHashtag;

public class GetTrendingHashtagQueryHandler : IRequestHandler<GetTrendingHashtagQuery, Result<List<HashtagVM>>>
{
    private readonly IHashtagService _hashtagService;
    private readonly ILogger<GetTrendingHashtagQueryHandler> _logger;

    public GetTrendingHashtagQueryHandler(IHashtagService hashtagService,
        ILogger<GetTrendingHashtagQueryHandler> logger)
    {
        _hashtagService = hashtagService;
        _logger = logger;
    }

    public async Task<Result<List<HashtagVM>>> Handle(GetTrendingHashtagQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _hashtagService.GetTrendingHashtagAsync(request.PageQuery.PageNumber);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetTrendingHashtagQuery>.QueryExecutedSuccessfully);

                return Result<List<HashtagVM>>.Success(response.ToList());
            }
            _logger.LogError(QueryMessages<GetTrendingHashtagQuery>.FailedToExecuteQuery);

            return Result<List<HashtagVM>>.Failure(QueryMessages<GetTrendingHashtagQuery>.FailedToExecuteQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetTrendingHashtagQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<List<HashtagVM>>.Failure(QueryMessages<GetTrendingHashtagQuery>.FailedToExecuteQueryException);
        }
    }
}
