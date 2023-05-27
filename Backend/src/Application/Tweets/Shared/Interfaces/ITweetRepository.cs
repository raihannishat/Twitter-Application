using Application.Common.Interfaces;

namespace Application.Tweets.Shared.Interfaces;

public interface ITweetRepository : IRepository<Tweet>
{
    //Task DeleteTweetCommentAsync(string tweetId, string commentId);

    Task<IEnumerable<Tweet>> GetTweetByHashtag(string name, int pageNumber, int pageSize);

    IEnumerable<Tweet> GetUserTweet(string userId);
}
