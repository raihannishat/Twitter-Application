namespace Application.Retweets.Shared.Service;

public class RetweetService : IRetweetService
{
    private readonly IRetweetRepository _retweetRepository;
    private readonly IUserTimelineRepository _userTimelineRepository;
    private readonly ITweetRepository _tweetRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;

    public RetweetService(IRetweetRepository retweetRepository,
        IUserTimelineRepository userTimelineRepository,
        ITweetRepository tweetRepository,
        ICurrentUserService currentUserService,
        INotificationService notificationService)
    {
        _retweetRepository = retweetRepository;
        _userTimelineRepository = userTimelineRepository;
        _tweetRepository = tweetRepository;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
    }

    public async Task<(bool IsRetweetedByCurrentUser, int Retweets)?> RetweetAsync(string tweetId)
    {
        (bool IsRetweetedByCurrentUser, int Retweets) result = (false, 0);

        var currentUserId = _currentUserService.UserId;

        var tweetObj = await _tweetRepository.FindByIdAsync(tweetId);

        if (tweetObj == null)
        {
            return null!;
        }

        if (!await IsRetweetedByUser(tweetId, currentUserId))
        {
            await HandleRetweetCreation(tweetObj, currentUserId);
        }
        else
        {
            await HandleRetweetRemoval(tweetObj, currentUserId);
        }

        await _tweetRepository.ReplaceOneAsync(tweetObj);

        result.Retweets = tweetObj.Retweets;
        
        result.IsRetweetedByCurrentUser = IsRetweetedByCurrentUser(tweetObj.Id, currentUserId);

        return (result.IsRetweetedByCurrentUser, result.Retweets);
    }

    public async Task<bool> IsRetweetedByUser(string tweetId, string userId)
    {
        var retweetObject = await _retweetRepository.FindOneByMatchAsync(x => x.TweetId == tweetId && x.UserId == userId);
        
        return retweetObject != null;
    }

    private async Task HandleRetweetCreation(Tweet tweetObj, string currentUserId)
    {
        await CreateRetweet(tweetObj);
        
        await _notificationService.CreateNotificationAsync(tweetObj, TweetConstants.RetweetAction);

        if (tweetObj.UserId == currentUserId)
        {
            await _userTimelineRepository.DeleteOneAsync(x => x.UserId == currentUserId && x.TweetId == tweetObj.Id);
        }

        var userTimeline = new UserTimeline
        {
            UserId = currentUserId,
            TweetId = tweetObj.Id,
            CreatedAt = DateTime.Now
        };

        await _userTimelineRepository.InsertOneAsync(userTimeline);

        tweetObj.Retweets++;
    }

    private async Task HandleRetweetRemoval(Tweet tweetObj, string currentUserId)
    {
        await _retweetRepository.DeleteOneAsync(x => x.TweetId == tweetObj.Id && x.UserId == currentUserId);

        if (tweetObj.UserId == currentUserId)
        {
            var timeline = await _userTimelineRepository.FindOneByMatchAsync(x => x.UserId == currentUserId && x.TweetId == tweetObj.Id);
            
            timeline.CreatedAt = tweetObj.CreatedAt;
            
            await _userTimelineRepository.ReplaceOneAsync(timeline);
        }
        else
        {
            await _userTimelineRepository.DeleteOneAsync(x => x.UserId == currentUserId && x.TweetId == tweetObj.Id);
        }

        tweetObj.Retweets--;
    }

    private bool IsRetweetedByCurrentUser(string tweetId, string currentUserId)
    {
        return _retweetRepository.FindOneByMatchAsync(x => x.TweetId == tweetId && x.UserId == currentUserId).Result != null;
    }

    private async Task CreateRetweet(Tweet tweetObj)
    {
        var retweetEntity = new Retweet
        {
            UserId = _currentUserService.UserId,
            TweetId = tweetObj.Id
        };

        await _retweetRepository.InsertOneAsync(retweetEntity);
    }
}
