//using Application.Tweets.Shared.Models;
//using NReJSON;
//using StackExchange.Redis;
//using System.Text.Json;

//namespace Infrastructure.Persistence.RedisCaching;

//public class TweetCacheService : ITweetCacheService
//{
//    private readonly ICurrentUserService _currentUserService;
//    public readonly ConnectionMultiplexer connectionMultiplexer;
//    private readonly IRedisSettings _redisSettings;
//    private readonly IUserRepository _userRepository;
//    private readonly IMapper _mapper;
//    private string _pattern;

//    public TweetCacheService(IRedisSettings redisSettings, ICurrentUserService currentUserService, IMapper mapper, IUserRepository userRepository)
//    {
//        _redisSettings = redisSettings;
//        connectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
//        {
//            EndPoints = { _redisSettings.Endpoint },
//            User = _redisSettings.User,
//            Password = _redisSettings.Password
//        });
//        _currentUserService = currentUserService;
//        _pattern = $"{_currentUserService.UserId}_HomeTimeline";
//        _mapper = mapper;
//        _userRepository = userRepository;
//    }

//    public async Task InsertTweetAsync(TweetDto tweet)
//    {
//        var db = connectionMultiplexer.GetDatabase();
        
//        var key = $"{tweet.Id}_Tweet";

//        var tweetJsonOBj = JsonSerializer.Serialize(tweet);

//        await db.JsonSetAsync(key, tweetJsonOBj);

//        //var res = db.SetScan("pos", "*as", 100, cursor: 0);

//        //var ans = (IScanningCursor)(res);
//    }

//    public async Task UpdateTweetAsync(TweetDto tweet)
//    {
//        var db = connectionMultiplexer.GetDatabase();
        
//        var key = $"{tweet.Id}_Tweet";
        
//        var tweetJsonOBj = JsonSerializer.Serialize(tweet);

//        await db.JsonSetAsync(key, tweetJsonOBj);
//    }


//    public async Task DeleteTweetAsync(string tweetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweetId}_Tweet";

//        await db.JsonDeleteAsync(key);
//    }

//    public async Task CreateUserTimeline(TweetDto tweet)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{_currentUserService.UserId}_UserTimeline";

//        var score = new DateTimeOffset(tweet.CreatedAt).ToUnixTimeSeconds();

//        await db.SortedSetAddAsync(key, tweet.Id, score);
//    }

//    public async Task DeleteTweetIdFromUserTimeline(TweetDto tweet)
//    {
//        var db = connectionMultiplexer.GetDatabase();
        
//        var key = $"{_currentUserService.UserId}_UserTimeline";

//        await db.SortedSetRemoveAsync(key, tweet.Id);

//    }

//    public async Task<List<TweetViewModel>> GetHomeTimelineTweets(int start, int end)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var currentUserId = _currentUserService.UserId; 
//        var tweetIds = await db.SortedSetRangeByRankAsync($"{currentUserId}_HomeTimeline", start, end);
//        var tweetIdList = tweetIds.ToStringArray();
        
//        if (!tweetIds.Any())
//        {
//            return new List<TweetViewModel>();
//        }

//        var tweetViewModelList = new List<TweetViewModel>();

//        foreach (var tweeId in tweetIdList)
//        {
//            var tweetString = db.JsonGet($"{tweeId}_Tweet", ".").ToString();
            
//            if (!tweetString.IsNullOrEmpty())
//            {
//                var tweetDto = JsonSerializer.Deserialize<TweetDto>(tweetString!);

//                var tweetViewModel = _mapper.Map<TweetViewModel>(tweetDto);

//                var isUserBlocked = await IsBlocked(currentUserId, tweetViewModel.UserId!);

//                var isCurrentUserBlocked = await IsBlocked(tweetViewModel.UserId!, currentUserId);

//                if (!isUserBlocked && !isCurrentUserBlocked)
//                {
//                    var user = await _userRepository.FindByIdAsync(tweetViewModel.UserId!);

//                    tweetViewModel.UserName = user.Name;

//                    var isLiked = db.SetScan($"{tweetDto!.Id}_Likes", $"*{currentUserId}", 100, cursor: 0);

//                    var isRetweeted = db.SetScan($"{tweetDto!.Id}_Retweets", $"*{currentUserId}", 100, cursor: 0);

//                    if (isLiked.Any())
//                    {
//                        tweetViewModel.IsLikedByCurrentUser = true;
//                    }

//                    if (isRetweeted.Any())
//                    {
//                        tweetViewModel.IsRetweetedByCurrentUser = true;
//                    }

//                    tweetViewModelList.Add(tweetViewModel);
//                }

                
//            }
//        }
//        return tweetViewModelList;
//    }

//    public async Task InsertHashtags(List<string> hashtags)
//    {
//        var db = connectionMultiplexer.GetDatabase();
        
//        var key = "Search";

//        var list = new List<RedisValue>();

//        foreach (var tag in hashtags)
//        {
//            list.Add(tag);
//        }

//        await db.SetAddAsync(key, list.ToArray());
//    }

//    public async Task InsertHomeTimeline(string userId, TweetDto tweet)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{userId}_HomeTimeline";

//        var score = new DateTimeOffset(tweet.CreatedAt).ToUnixTimeSeconds();

//        await db.SortedSetAddAsync(key, tweet.Id, score);
//    }

//    public async Task UpdateTweetCommentCount(TweetDto tweet, int value)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweet.Id}_Tweet";

//        await db.JsonIncrementNumberAsync(key, ".Comment", value);
//    }

//    public async Task<bool> DeleteUserIdFromLikes(string userId, string tweetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();
        
//        var key = $"{tweetId}_Likes";

//        var res = await db.SetRemoveAsync(key, userId);
        
//        return res;
//    }

//    public async Task AddUserIdInLikes(string userId, string tweetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweetId}_Likes";

//        await db.SetAddAsync(key, userId);
//    }

//    public async Task UpdateTweetLikesCount(string tweetId, int value)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweetId}_Tweet";

//        await db.JsonIncrementNumberAsync(key, ".Likes", value);
//    }

//    public async Task UpdateTweetRetweetCount(string tweetId, int value)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweetId}_Tweet";

//        await db.JsonIncrementNumberAsync(key, ".Retweets", value);
//    }

//    public async Task<bool> DeleteUserIdFromRetweets(string userId, string tweetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweetId}_Retweets";

//        var res = await db.SetRemoveAsync(key, userId);

//        return res;
//    }

//    public async Task AddUserIdInRetweets(string userId, string tweetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweetId}_Retweets";

//        await db.SetAddAsync(key, userId);
//    }

//    public async Task DeleteTweetLikes(string tweetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweetId}_Likes";

//        await db.KeyDeleteAsync(key);

//    }

//    public async Task DeleteTweetRetweets(string tweetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweetId}_Retweets";

//        await db.KeyDeleteAsync(key);
//    }

//    public async Task InsertRetweetIdInUserTimeline(TweetDto tweet)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{_currentUserService.UserId}_UserTimeline";

//        var score = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

//        await db.SortedSetAddAsync(key, tweet.Id, score);
//    }

//    public async Task<bool> RemoveUserIdFromBlock(string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{_currentUserService.UserId}_Blocks";

//        var res = await db.SetRemoveAsync(key, targetId);
        
//        return res;
//    }

//    public async Task AddTargetUserInBlock(string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{_currentUserService.UserId}_Blocks";

//        await db.SetAddAsync(key, targetId);
//    }

//    public async Task<bool> RemoveTargetIdFromFollowing(string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{_currentUserService.UserId}_Followings";

//        var result = await db.SortedSetRemoveAsync(key, targetId);
        
//        return result;

//    }
    
//    public async Task RemoveUserFromTargetUserFollower(string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{targetId}_Followers";

//        await db.SortedSetRemoveAsync(key, _currentUserService.UserId);
//    }

//    public async Task RemoveCelebrityFromFollowing(string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{_currentUserService.UserId}_Celebrity_Followings";

//        await db.SetRemoveAsync(key, targetId);
//    }

//    public async Task AddCelebrityInFollowing(string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{_currentUserService.UserId}_Celebrity_Followings";

//        await db.SetAddAsync(key, targetId);
//    }

//    public async Task AddTargetIdInFollowing(string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{_currentUserService.UserId}_Followings";

//        var score = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

//        await db.SortedSetAddAsync(key, targetId, score);
//    }

//    public async Task AddUserInTargetUserFollower(string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{targetId}_Followers";

//        var score = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

//        await db.SortedSetAddAsync(key, _currentUserService.UserId, score);
//    }

//    public async Task<int> FollowersCount(string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{targetId}_Followers";

//        var res = await db.SortedSetLengthAsync(key);

//        return (int)res;
//    }

//    public async Task<int> FollowingsCount(string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{targetId}_Followings";

//        var res = await db.SortedSetLengthAsync(key);

//        return (int)res;
//    }

//    public async Task<List<string>> GetFollowersId(string userId, int start, int end)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var followersId = await db.SortedSetRangeByRankAsync($"{userId}_Followers", start, end);

//        if (!followersId.Any())
//        {
//            return new List<string>();
//        }

//        var list = followersId.ToStringArray();
        
//        return list.ToList()!;

//    }

//    public async Task<List<string>> GetFollowingsId(string userId, int start, int end)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var followingsId = await db.SortedSetRangeByRankAsync($"{userId}_Followings", start, end);

//        if (!followingsId.Any())
//        {
//            return new List<string>();
//        }

//        var list = followingsId.ToStringArray();

//        return list.ToList()!;
//    }


//    public async Task<bool> IsFollowing(string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();
        
//        var key = $"{_currentUserService.UserId}_Followings";

//        var result =  db.SortedSetScan(key, $"*{targetId}", 100, cursor: 0);

//        if (result.Any())
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    public async Task<List<string>> GetBlockIds(string userId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{userId}_Blocks";

//        var blockIds = await db.SetMembersAsync(key);

//        if(!blockIds.Any())
//        {
//            return new List<string>();
//        }
//        var res = blockIds.ToStringArray();

//        return res.ToList()!;
//    }

//    public async Task<bool> IsBlocked(string userId, string targetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{userId}_Blocks";

//        var blockId = db.SetScan(key, $"*{targetId}", 100, cursor: 0);

//        if (blockId.Any())
//        {
//            return true;
//        }    
//        else
//        {
//            return false;
//        }
//    }

//    public async Task<int> GetTweetCommentCount(string tweetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweetId}_Tweet";

//        var count = await db.JsonGetAsync(key, ".Comment");

//        return (int)count;
//    }

//    public async Task<bool> IsLikeByCurrentUser(string tweetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweetId}_Likes";

//        var isLiked = db.SetScan(key, $"*{_currentUserService.UserId}", 100, cursor: 0);

//        if (isLiked.Any())
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    public async Task<bool> IsRetweetedByCurrentUser(string tweetId)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = $"{tweetId}_Retweets";

//        var isRetweeted = db.SetScan(key, $"*{_currentUserService.UserId}", 100, cursor: 0);
        
//        if (isRetweeted.Any())
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    public async Task<List<string>> GetSuggestionFromSet(string keyword)
//    {
//        var db = connectionMultiplexer.GetDatabase();

//        var key = "Search";

//        var result = db.SetScan(key, $"*{keyword}*", 100, cursor: 0);

//        if (!result.Any())
//        {
//            return new List<string>();
//        }
        
//        var suggestions = new List<string>();

//        foreach (var item in result)
//        {
//            suggestions.Add(item.ToString());
//        }

//        return suggestions;
//    }


//    //public string GetJsonValue(string key)
//    //{
//    //    var db = connectionMultiplexer.GetDatabase();

//    //    var tweetString = db.JsonGet($"{key}_Tweet").ToString();
//    //    if (tweetString.IsNullOrEmpty())
//    //    {
//    //        Console.WriteLine("empty string");
//    //    }

//    //    return tweetString.ToString();
//    //}
//}
