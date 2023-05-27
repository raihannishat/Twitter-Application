namespace Application.Tweets.Shared.Service;

public class TweetService : ITweetService
{
    private readonly ITweetRepository _tweetRepository;
    private readonly IUserTimelineRepository _userTimelineRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly ILikeService _likeService;
    private readonly IRetweetService _retweetService;

    public TweetService(ITweetRepository tweetRepository,
        IUserTimelineRepository userTimelineRepository,
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        ILikeService likeService,
        IRetweetService retweetService)
    {
        _currentUserService = currentUserService;
        _tweetRepository = tweetRepository;
        _userTimelineRepository = userTimelineRepository;
        _userRepository = userRepository;
        _likeService = likeService;
        _retweetService = retweetService;
    }

    public async Task CreateTweetAsync(Tweet tweet)
    {
        await _tweetRepository.InsertOneAsync(tweet);
    }

    public async Task<bool?> DeleteTweetAsync(string tweetId)
    {
        var userId = _currentUserService.UserId;

        var tweet = await _tweetRepository.FindByIdAsync(tweetId);

        if (tweet == null || tweet.UserId != _currentUserService.UserId)
        {
            return null!;
        }

        await _tweetRepository.DeleteByIdAsync(tweet.Id);

        await _userTimelineRepository
            .DeleteOneAsync(x => x.TweetId == tweet.Id && x.UserId == userId);

        return true;
    }

    public async Task<TweetViewModel> GetTweetByIdAsync(string tweetId, string tweetOwnerId)
    {
        var tweet = await GetTweetAsync(tweetId);

        if (tweet == null)
        {
            return null!;
        }

        var tweetOwner = await _userRepository.FindByIdAsync(tweetOwnerId);

        var tweetCreator = await _userRepository.FindByIdAsync(tweet.UserId!);

        if (tweetOwner == null || tweetCreator == null)
        {
            return null!;
        }

        var tweetVm = await MapToTweetViewModel(tweet, tweetOwner, tweetCreator);

        return tweetVm;
    }

    private async Task<Tweet> GetTweetAsync(string tweetId)
    {
        return await _tweetRepository.FindByIdAsync(tweetId);
    }

    private async Task<TweetViewModel> MapToTweetViewModel(Tweet tweet, User tweetOwner, User tweetCreator)
    {
        var tweetViewModel = new TweetViewModel
        {
            Id = tweet.Id,
            Content = tweet.Content,
            CreatedAt = tweet.CreatedAt,
            Likes = tweet.Likes,
            Retweets = tweet.Retweets,
            Comments = tweet.Comments,
            UserId = tweetOwner.Id,
            UserName = tweetOwner.Name,
            Image = tweetOwner.Image,
            TweetCreatorId = tweetCreator.Id,
            TweetCreatorName = tweetCreator.Name,
            TweetCreatorImage = tweetCreator.Image,
            Edited = tweet.Edited,
        };

        tweetViewModel.IsLikedByCurrentUser = await _likeService.IsTweetLikedByUser(tweet.Id, _currentUserService.UserId);

        tweetViewModel.IsRetweetedByCurrentUser = await _retweetService.IsRetweetedByUser(tweet.Id, _currentUserService.UserId);

        return tweetViewModel;
    }
}
