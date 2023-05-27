namespace Application.Tweets.Shared.Service;

public interface ITweetService
{
    Task CreateTweetAsync(Tweet tweet);
    Task<bool?> DeleteTweetAsync(string tweetId);
    Task<TweetViewModel> GetTweetByIdAsync(string tweetId, string tweetOwnerId);
}