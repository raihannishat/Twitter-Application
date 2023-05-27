//using Application.Tweets.Shared.Models;

//namespace Application.Common.Interfaces;

//public interface ITweetCacheService
//{
//    Task InsertTweetAsync(TweetDto tweet);
//    Task UpdateTweetAsync(TweetDto tweet);
//    Task DeleteTweetAsync(string tweetId);
//    Task DeleteTweetLikes(string tweetId);
//    Task DeleteTweetRetweets(string tweetId);
//    Task UpdateTweetCommentCount(TweetDto tweet, int value);
//    Task UpdateTweetLikesCount(string tweetId, int value);
//    Task UpdateTweetRetweetCount(string tweetId, int value);
//    Task CreateUserTimeline(TweetDto tweet);
//    Task DeleteTweetIdFromUserTimeline(TweetDto tweet);
//    Task InsertRetweetIdInUserTimeline(TweetDto tweet);
//    Task InsertHomeTimeline(string userId, TweetDto tweet);
//    Task InsertHashtags(List<string> hashtags);
//    Task<bool> DeleteUserIdFromLikes(string userId, string tweetId);
//    Task<bool> DeleteUserIdFromRetweets(string userId, string tweetId);
//    Task AddUserIdInLikes(string userId, string tweetId);
//    Task AddUserIdInRetweets(string userId, string tweetId);
//    Task<bool> RemoveUserIdFromBlock(string targetId);
//    Task AddTargetUserInBlock(string targetId);
//    Task<List<TweetViewModel>> GetHomeTimelineTweets(int start, int end);
//    Task<bool> RemoveTargetIdFromFollowing(string targetId);
//    Task AddTargetIdInFollowing(string targetId);
//    Task RemoveUserFromTargetUserFollower(string userId);
//    Task AddUserInTargetUserFollower(string userId);
//    Task RemoveCelebrityFromFollowing(string targetId);
//    Task AddCelebrityInFollowing(string targetId);
//    Task<int> FollowersCount(string targetId);
//    Task<int> FollowingsCount(string targetId);
//    Task<List<string>> GetFollowersId(string userId, int start, int end);
//    Task<List<string>> GetFollowingsId(string userId, int start, int end);
//    Task<bool> IsFollowing(string targetId);
//    Task<bool> IsBlocked(string userId, string targetId);
//    Task<List<string>> GetBlockIds(string userId);
//    Task<int> GetTweetCommentCount(string tweetId);
//    Task<bool> IsLikeByCurrentUser(string tweetId);
//    Task<bool> IsRetweetedByCurrentUser(string tweetId);
//    Task<List<string>> GetSuggestionFromSet(string keyword);
//    //string GetJsonValue(string key);
//}
