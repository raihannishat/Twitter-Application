namespace Application.Tweets.Shared.Service;

public interface ITimelineService
{
    Task<IList<TweetViewModel>> GetHomeTimelineAsync(int pageNumber);
    Task<IList<TweetViewModel>> GetUserTimelineAsync(string userId, int pageNumber);
    Task InsertTweetToFollowersHomeTimeline(Tweet tweet, string currentUserId);
    Task CreateUserTimeline(Tweet tweet);
    Task DeleteTweetFromUserHomeTimeline(string targetUserId, string tweetOwnerId);
    Task AddUserTweetsToTimeline(User currentUser, User targetUser);
}
