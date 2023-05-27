namespace Application.Tweets.Shared.Service;

public class TimelineService : ITimelineService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IHomeTimelineRepository _homeTimelineRepository;
    private readonly IUserTimelineRepository _userTimelineRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILikeService _likeService;
    private readonly IRetweetService _retweetService;
    private readonly IBlockUserService _blockService;
    private readonly IRetweetRepository _retweetRepository;
    private readonly IBlockRepository _blockRepository;
    private readonly ITweetRepository _tweetRepository;
    private readonly ITweetViewModelFactory _tweetViewModelFactory;
    private readonly IFollowRepository _followRepository;

    public TimelineService(ICurrentUserService currentUserService,
        IHomeTimelineRepository homeTimelineRepository,
        IUserTimelineRepository userTimelineRepository,
        IUserRepository userRepository,
        IRetweetRepository retweetRepository,
        IBlockRepository blockRepository,
        ITweetRepository tweetRepository,
        ITweetViewModelFactory tweetViewModelFactory,
        IFollowRepository followRepository,
        ILikeService likeService,
        IRetweetService retweetService,
        IBlockUserService blockService)
    {
        _currentUserService = currentUserService;
        _homeTimelineRepository = homeTimelineRepository;
        _userTimelineRepository = userTimelineRepository;
        _userRepository = userRepository;
        _retweetRepository = retweetRepository;
        _blockRepository = blockRepository;
        _tweetRepository = tweetRepository;
        _tweetViewModelFactory = tweetViewModelFactory;
        _followRepository = followRepository;
        _likeService = likeService;
        _retweetService = retweetService;
        _blockService = blockService;
    }

    public async Task<IList<TweetViewModel>> GetHomeTimelineAsync(int pageNumber)
    {
        var currentUserId = _currentUserService.UserId;

        var homeTimelines = _homeTimelineRepository.GetTweetByDescendingTime(x => x.UserId == currentUserId, pageNumber);

        if (!homeTimelines.Any())
        {
            return new List<TweetViewModel>();
        }

        var homeTimelineTweets = await GetFilteredHomeTimeline(homeTimelines.ToList());

        var homeTimelineTweetViewModels = homeTimelineTweets.Select(x => CreateTweetViewModelAsync(x).Result).ToList();

        return homeTimelineTweetViewModels;
    }

    public async Task InsertTweetToFollowersHomeTimeline(Tweet tweet, string currentUserId)
    {
        var followerList = await _followRepository.GetFollowers(x => x.FollowedId == currentUserId);

        var followerIdList = followerList.Select(x => x.FollowerId).ToList();

        foreach (var followerId in followerIdList)
        {
            var homeTimeline = new HomeTimeline
            {
                UserId = followerId,
                TweetOwnerId = currentUserId,
                TweetId = tweet.Id,
                CreatedAt = tweet.CreatedAt
            };

            await _homeTimelineRepository.InsertOneAsync(homeTimeline);
        }
    }

    public async Task CreateUserTimeline(Tweet tweet)
    {
        var timeline = new UserTimeline
        {
            TweetId = tweet.Id,
            UserId = _currentUserService.UserId,
            CreatedAt = tweet.CreatedAt
        };

        await _userTimelineRepository.InsertOneAsync(timeline);
    }

    public async Task<IList<TweetViewModel>> GetUserTimelineAsync(string userId, int pageNumber)
    {
        var user = await _userRepository.FindByIdAsync(userId);

        if (user == null)
        {
            return new List<TweetViewModel>();
        }

        var userTimelines = _userTimelineRepository.GetTweetByDescendingTime(x => x.UserId == userId, pageNumber);

        if (!userTimelines.Any())
        {
            return new List<TweetViewModel>();
        }

        var tweetList = new List<TweetViewModel>();

        var currentUserId = _currentUserService.UserId;

        foreach (var userTimeline in userTimelines)
        {
            var tweet = await _tweetRepository.FindByIdAsync(userTimeline.TweetId);

            if (tweet == null)
            {
                continue;
            }

            if (await IsTweetShouldVisible(tweet, currentUserId))
            {
                var tweetVm = await _tweetViewModelFactory.CreateTweetViewModelAsync(tweet, user);
                tweetList.Add(tweetVm);
            }
        }

        return tweetList;
    }

    public async Task DeleteTweetFromUserHomeTimeline(string targetUserId, string tweetOwnerId)
    {
        await _homeTimelineRepository.DeleteUserHomeTimeline(x =>
            x.UserId == targetUserId && x.TweetOwnerId == tweetOwnerId);
    }

    public async Task AddUserTweetsToTimeline(User currentUser, User targetUser)
    {
        var userTweets = _tweetRepository.GetUserTweet(targetUser.Id);

        if (userTweets.Any())
        {
            foreach (var tweet in userTweets)
            {
                var homeTimeline = new HomeTimeline
                {
                    UserId = currentUser.Id,
                    TweetId = tweet.Id,
                    TweetOwnerId = tweet.UserId!,
                    CreatedAt = tweet.CreatedAt,
                };

                await _homeTimelineRepository.InsertOneAsync(homeTimeline);
            }
        }
    }

    private async Task<bool> IsTweetShouldVisible(Tweet tweet, string currentUserId)
    {
        var isCurrentUserBlocked = await _blockRepository.FindOneByMatchAsync(x => x.BlockedId == currentUserId && x.BlockedById == tweet.UserId);
        var isTweetCreatorBlocked = await _blockRepository.FindOneByMatchAsync(x => x.BlockedId == tweet.UserId && x.BlockedById == currentUserId);

        return isCurrentUserBlocked == null && isTweetCreatorBlocked == null;
    }

    private async Task<List<HomeTimeline>> GetFilteredHomeTimeline(List<HomeTimeline> homeTimelines)
    {
        var homeTimelineTweets = new List<HomeTimeline>();

        foreach (var homeTimeline in homeTimelines)
        {
            var tweetObj = await _tweetRepository.FindByIdAsync(homeTimeline.TweetId);

            if (tweetObj == null)
            {
                continue;
            }

            var tweetCreator = await _userRepository.FindByIdAsync(tweetObj.UserId!);

            var tweetOwner = await _userRepository.FindByIdAsync(homeTimeline.TweetOwnerId);

            if (tweetOwner == null || tweetCreator == null)
            {
                continue;
            }

            var isBlockedByCurrentUser = await _blockService.IsBlockAsync(homeTimeline.TweetOwnerId, _currentUserService.UserId);

            var isBlockedByTweetOwner = await _blockService.IsBlockAsync(_currentUserService.UserId, homeTimeline.TweetOwnerId);

            if (isBlockedByCurrentUser || isBlockedByTweetOwner)
            {
                continue;
            }

            homeTimelineTweets.Add(homeTimeline);
        }

        return homeTimelineTweets;
    }

    private async Task<TweetViewModel> CreateTweetViewModelAsync(HomeTimeline homeTimeline)
    {
        var tweetObj = await _tweetRepository.FindByIdAsync(homeTimeline.TweetId);
        var tweetOwner = await _userRepository.FindByIdAsync(homeTimeline.TweetOwnerId);
        var tweetCreator = await _userRepository.FindByIdAsync(tweetObj.UserId!);

        var tweetVm = new TweetViewModel()
        {
            Id = tweetObj.Id,
            Content = tweetObj.Content,
            CreatedAt = tweetObj.CreatedAt,
            Likes = tweetObj.Likes,
            Retweets = tweetObj.Retweets,
            Comments = tweetObj.Comments,
            UserId = tweetOwner.Id,
            UserName = tweetOwner.Name,
            Image = tweetOwner.Image,
            TweetCreatorId = tweetCreator.Id,
            TweetCreatorName = tweetCreator.Name,
            TweetCreatorImage = tweetCreator.Image,
            CanDelete = false
        };

        tweetVm.IsLikedByCurrentUser = await _likeService.IsTweetLikedByUser(homeTimeline.TweetId, _currentUserService.UserId);

        tweetVm.IsRetweetedByCurrentUser = await _retweetService.IsRetweetedByUser(homeTimeline.TweetId, _currentUserService.UserId);

        tweetVm.IsRetweeted = _retweetRepository.FindOneByMatchAsync(x =>
            x.TweetId == homeTimeline.TweetId && x.UserId == homeTimeline.TweetOwnerId).Result != null;

        return tweetVm;
    }
}
