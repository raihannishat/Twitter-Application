namespace Application.Tweets.Queries.GetUserTimelineTweets;

public class GetUserTimelineQuery : IRequest<Result<List<TweetViewModel>>>
{
    public string UserId { get; set; }
    public PaginationQueryRequest PageQuery { get; set; }
}
