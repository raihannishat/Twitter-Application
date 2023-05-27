using Application.Tweets.Shared.Models;

namespace Application.Tweets.Queries.GetHashtagTweets;

public class GetHashtagTweetsQuery : IRequest<Result<List<TweetViewModel>>>
{
    public PaginationQueryRequest PageQuery { get; set; }

    public GetHashtagTweetsQuery(PaginationQueryRequest paginationQuery) => PageQuery = paginationQuery;
}
