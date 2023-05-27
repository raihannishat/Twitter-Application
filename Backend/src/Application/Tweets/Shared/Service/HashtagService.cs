namespace Application.Tweets.Shared.Service;

public class HashtagService : IHashtagService
{
    private readonly ITweetRepository _tweetRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHashtagRepository _hashtagRepository;
    private readonly IBlockUserService _blockService;
    private readonly IRetweetService _retweetService;
    private readonly IUserRepository _userRepository;
    private readonly ISearchRepository _searchRepository;
    private readonly ILikeService _likeService;
    private readonly IMapper _mapper;

    public HashtagService(ISearchRepository searchRepository,
        ITweetRepository tweetRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IHashtagRepository hashtagRepository,
        IMapper mapper,
        IBlockUserService blockService,
        IRetweetService retweetService,
        ILikeService likeService)
    {
        _tweetRepository = tweetRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _hashtagRepository = hashtagRepository;
        _searchRepository = searchRepository;
        _mapper = mapper;
        _blockService = blockService;
        _retweetService = retweetService;
        _likeService = likeService;
    }

    public List<string> ExtractHashTag(string content)
    {
        var regex = new Regex(@"#\w+");

        var matches = regex.Matches(content);

        var hashtags = new HashSet<string>();

        if (matches.Count == 0)
        {
            return new List<string>();
        }

        foreach (var match in matches)
        {
            hashtags.Add(match.ToString()!.ToLower());
        }

        return hashtags.ToList();
    }

    public async Task<IList<HashtagVM>> GetHashTagAsync(string tagName)
    {
        // We have this option in MongoDB Cloud.
        //var tagNames = _searchRepository.GetHashtagByFuzzySearch(request.TagName);

        var tagNames = await _searchRepository.GetHashtagByRegex(tagName);

        if (tagNames == null)
        {
            return new List<HashtagVM>();
        }

        var tags = new List<HashtagVM>();

        foreach (var tag in tagNames)
        {
            tags.Add(_mapper.Map<HashtagVM>(tag));
        }

        return tags;
    }

    public async Task<IList<HashtagVM>> GetTrendingHashtagAsync(int pageNumber)
    {
        var hashtags = await _searchRepository.GetHashtagWithPagination(pageNumber);

        var hashtagsList = new List<HashtagVM>();

        foreach (var hashtag in hashtags)
        {
            hashtagsList.Add(_mapper.Map<HashtagVM>(hashtag));
        }

        return hashtagsList;
    }

    public async Task InsertHashtag(List<string> hashtags, Tweet tweet)
    {
        foreach (var tag in hashtags)
        {
            var tagEntity = new Hashtag
            {
                TagName = tag,
                TweetId = tweet.Id,
                UserId = _currentUserService.UserId,
                CreatedAt = tweet.CreatedAt
            };

            await _hashtagRepository.InsertOneAsync(tagEntity);
        }
    }

    public async Task InsertHashtagInSearchTable(List<string> hashtags)
    {
        foreach (var tag in hashtags)
        {
            var tagExist = await _searchRepository
                .FindOneByMatchAsync(x => x.HashTag == tag);

            if (tagExist == null)
            {
                var tagEntity = new Search
                {
                    HashTag = tag
                };

                await _searchRepository.InsertOneAsync(tagEntity);
            }
        }
    }

    public async Task ProcessTweetsHashtag(Tweet tweet)
    {
        var hashtags = ExtractHashTag(tweet.Content!);

        if (hashtags.Count > 0)
        {
            await InsertHashtag(hashtags, tweet);
            await InsertHashtagInSearchTable(hashtags);
        }
    }

    public async Task<IList<TweetViewModel>> GetHashtagTweetsAsync(string keyword, int pageNumber)
    {
        var hashTagName = keyword;

        if (string.IsNullOrEmpty(hashTagName))
        {
            return null!;
        }

        var hashtags = _hashtagRepository.GetHashtagTweetByDescendingTime(x => x.TagName == hashTagName, pageNumber);

        if (!hashtags.Any())
        {
            return new List<TweetViewModel>();
        }

        hashtags = await GetFilteredHashtagsAsync(hashtags);

        var tweetVMList = await GetTweetViewModelsAsync(hashtags);

        return tweetVMList;
    }

    private async Task<List<Hashtag>> GetFilteredHashtagsAsync(IEnumerable<Hashtag> hashtags)
    {
        var validHashtags = new List<Hashtag>();

        foreach (var hashtag in hashtags)
        {
            var isCurrentUserBlocked = await _blockService.IsBlockAsync(_currentUserService.UserId, hashtag.UserId);

            var isUserBlocked = await _blockService.IsBlockAsync(hashtag.UserId, _currentUserService.UserId);

            if (!isCurrentUserBlocked && !isUserBlocked)
            {
                validHashtags.Add(hashtag);
            }
        }

        return validHashtags;
    }

    private async Task<List<TweetViewModel>> GetTweetViewModelsAsync(IEnumerable<Hashtag> hashtags)
    {
        var tweetCollection = _tweetRepository.GetCollection();
        var userCollection = _userRepository.GetCollection();

        var tweetList = from hashtag in hashtags
                        join tweet in tweetCollection on hashtag.TweetId equals tweet.Id
                        select tweet;

        var tweets = from tweet in tweetList
                     join user in userCollection on tweet.UserId equals user.Id
                     select new TweetViewModel
                     {
                         Id = tweet.Id,
                         Content = tweet.Content,
                         CreatedAt = tweet.CreatedAt,
                         Likes = tweet.Likes,
                         Retweets = tweet.Retweets,
                         Comments = tweet.Comments,
                         UserId = user.Id,
                         UserName = user.Name,
                         Image = user.Image,
                         Edited = tweet.Edited,
                         TweetCreatorId = user.Id,
                         TweetCreatorName = user.Name,
                         TweetCreatorImage = user.Image,
                     };

        var tweetVMList = new List<TweetViewModel>();

        foreach (var tweet in tweets)
        {
            var tweetViewModel = _mapper.Map<TweetViewModel>(tweet);

            tweetViewModel.IsLikedByCurrentUser = await _likeService.IsTweetLikedByUser(tweet.Id, _currentUserService.UserId);

            tweetViewModel.IsRetweetedByCurrentUser = await _retweetService.IsRetweetedByUser(tweet.Id, _currentUserService.UserId);

            tweetVMList.Add(tweetViewModel);
        }

        return tweetVMList;
    }
}
