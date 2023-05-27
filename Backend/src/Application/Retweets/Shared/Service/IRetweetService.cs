namespace Application.Retweets.Shared.Service;

public interface IRetweetService
{
    Task<(bool IsRetweetedByCurrentUser, int Retweets)?> RetweetAsync(string tweetId);
    Task<bool> IsRetweetedByUser(string tweetId, string userId);
}