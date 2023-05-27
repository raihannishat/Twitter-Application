using Application.Tweets.Shared.Models;

namespace Application.Tweets.Queries.GetHomeTimelineTweets;

public class GetHomeTimelineQuery : IRequest<Result<List<TweetViewModel>>>
{
    public PaginationQueryRequest PageQuery { get; set; }

    public GetHomeTimelineQuery(PaginationQueryRequest paginationQuery) => PageQuery = paginationQuery;
}
