namespace Application.Tweets.Commands.CreateTweet;

public class CreateTweetCommandHandler : IRequestHandler<CreateTweetCommand, Result<Unit>>
{
    private readonly ITweetService _tweetService;
    private readonly ILogger<CreateTweetCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    private readonly IHashtagService _hashtagService;
    private readonly ITimelineService _timelineService;

    public CreateTweetCommandHandler(
        ICurrentUserService currentUserService,
        ILogger<CreateTweetCommandHandler> logger,
        IHashtagService hashtagService,
        ITimelineService timelineService,
        ITweetService tweetService)
    {
        _currentUserService = currentUserService;
        _logger = logger;
        _hashtagService = hashtagService;
        _timelineService = timelineService;
        _tweetService = tweetService;
    }

    public async Task<Result<Unit>> Handle(CreateTweetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = _currentUserService.UserId;

            var tweetEntity = CreateTweetEntity(request, currentUserId);
            
            await _tweetService.CreateTweetAsync(tweetEntity);
            
            await _timelineService.CreateUserTimeline(tweetEntity);
            
            await _hashtagService.ProcessTweetsHashtag(tweetEntity);
            
            await _timelineService.InsertTweetToFollowersHomeTimeline(tweetEntity, currentUserId);

            _logger.LogInformation(CommandMessages<CreateTweetCommand>.CommandExecutedSuccessfully);

            return Result<Unit>.Success(Unit.Value);
        }
        catch(Exception ex)
        {
            _logger.LogError(CommandMessages<CreateTweetCommand>.FailedToExecuteCommandException, ex.Message);

            return Result<Unit>.Failure(CommandMessages<CreateTweetCommand>.FailedToExecuteCommandException);
        }

    }
    private Tweet CreateTweetEntity(CreateTweetCommand request, string currentUserId)
    {
        return new Tweet
        {
            UserId = currentUserId,
            Content = request.Content,
            CreatedAt = DateTime.Now,
        };
    }
}
