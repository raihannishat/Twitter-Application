namespace Application.Tweets.Queries.GetHomeTimelineTweets;

public class GetHomeTimelineQueryHandler : IRequestHandler<GetHomeTimelineQuery, Result<List<TweetViewModel>>>
{
    private readonly ITimelineService _timelineService;
    private readonly ILogger<GetHomeTimelineQueryHandler> _logger;

    public GetHomeTimelineQueryHandler(ITimelineService timelineService,
        ILogger<GetHomeTimelineQueryHandler> logger)
    {
        _timelineService = timelineService;
        _logger = logger;
    }

    public async Task<Result<List<TweetViewModel>>> Handle(GetHomeTimelineQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _timelineService.GetHomeTimelineAsync(request.PageQuery.PageNumber);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetHomeTimelineQuery>.QueryExecutedSuccessfully);

                return Result<List<TweetViewModel>>.Success(response.ToList());
            }
            _logger.LogError(QueryMessages<GetHomeTimelineQuery>.FailedToExecuteQuery);

            return Result<List<TweetViewModel>>.Failure(QueryMessages<GetHomeTimelineQuery>.FailedToExecuteQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetHomeTimelineQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<List<TweetViewModel>>.Failure(QueryMessages<GetHomeTimelineQuery>.FailedToExecuteQueryException);
        }
    }
}
