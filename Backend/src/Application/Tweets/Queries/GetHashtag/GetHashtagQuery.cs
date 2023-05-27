namespace Application.Tweets.Queries.GetHashtag;

public class GetHashtagQuery : IRequest<Result<List<HashtagVM>>>
{
    public string TagName { get; set; } = string.Empty;

    public GetHashtagQuery(string tagName)
    {
        TagName = tagName;
    }
}
