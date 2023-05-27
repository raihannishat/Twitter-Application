namespace Application.Tweets.Queries.GetTrendingHashtag;

public class GetTrendingHashtagQuery : IRequest<Result<List<HashtagVM>>>
{
    public PaginationQueryRequest PageQuery { get; set; }

    public GetTrendingHashtagQuery(PaginationQueryRequest paginationQuery) => PageQuery = paginationQuery;
}
