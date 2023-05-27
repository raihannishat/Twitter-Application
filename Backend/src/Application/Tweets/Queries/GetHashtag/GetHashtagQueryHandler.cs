namespace Application.Tweets.Queries.GetHashtag;

public class GetHashtagQueryHandler : IRequestHandler<GetHashtagQuery, Result<List<HashtagVM>>>
{
    private readonly IHashtagService _hashtagService;
    private readonly ILogger<GetHashtagQueryHandler> _logger;

    public GetHashtagQueryHandler(IHashtagService hashtagService,
        ILogger<GetHashtagQueryHandler> logger)
    {
        _hashtagService = hashtagService;
        _logger = logger;
    }

    public async Task<Result<List<HashtagVM>>> Handle(GetHashtagQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _hashtagService.GetHashTagAsync(request.TagName);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetHashtagQuery>.QueryExecutedSuccessfully);

                return Result<List<HashtagVM>>.Success(response.ToList());
            }

            _logger.LogError(QueryMessages<GetHashtagQuery>.FailedToExecuteQuery);

            return Result<List<HashtagVM>>.Failure(QueryMessages<GetHashtagQuery>.FailedToExecuteQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetHashtagQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<List<HashtagVM>>.Failure(QueryMessages<GetHashtagQuery>.FailedToExecuteQueryException);
        }
    }
}
