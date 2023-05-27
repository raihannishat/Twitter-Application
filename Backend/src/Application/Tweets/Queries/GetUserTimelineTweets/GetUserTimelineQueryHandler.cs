namespace Application.Tweets.Queries.GetUserTimelineTweets;

public class GetUserTimelineQueryHandler : IRequestHandler<GetUserTimelineQuery, Result<List<TweetViewModel>>>
{
    private readonly ITimelineService _timelineService;
    private readonly ILogger<GetUserTimelineQueryHandler> _logger;

    public GetUserTimelineQueryHandler(ITimelineService timelineService,
        ILogger<GetUserTimelineQueryHandler> logger)
    {
        _timelineService = timelineService;
        _logger = logger;
    }

    public async Task<Result<List<TweetViewModel>>> Handle(GetUserTimelineQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _timelineService
                .GetUserTimelineAsync(request.UserId!, request.PageQuery.PageNumber);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetUserTimelineQuery>.QueryExecutedSuccessfully);

                return Result<List<TweetViewModel>>.Success(response.ToList());
            }

            _logger.LogError(QueryMessages<GetUserTimelineQuery>.FailedToExecuteQuery);

            return Result<List<TweetViewModel>>.Failure(QueryMessages<GetUserTimelineQuery>.FailedToExecuteQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetUserTimelineQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<List<TweetViewModel>>.Failure(QueryMessages<GetUserTimelineQuery>.FailedToExecuteQueryException);
        }
    }
}
