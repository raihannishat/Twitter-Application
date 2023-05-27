namespace Application.Comments.Queries.GetComments;

public class GetCommentQuery : IRequest<Result<List<CommentViewModel>>>
{
    public string TweetId { get; set; } = string.Empty;
    public PaginationQueryRequest PageQuery { get; set; }

    public GetCommentQuery(PaginationQueryRequest paginationQuery, string tweetId)
    {
        TweetId = tweetId;
        PageQuery = paginationQuery;
    }
}
