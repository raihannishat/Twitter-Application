using Application.Tweets.Shared.Models;

namespace Application.Tweets.Queries.GetTweetById;

public class GetTweetByIdQuery : IRequest<Result<TweetViewModel>>
{
    public string TweetId { get; set; } = string.Empty;
    public string TweetOwnerId { get; set; } = string.Empty;

    public GetTweetByIdQuery(string tweetId, string tweetOwnerId)
    {
        TweetId = tweetId;
        TweetOwnerId = tweetOwnerId;
    }

}
