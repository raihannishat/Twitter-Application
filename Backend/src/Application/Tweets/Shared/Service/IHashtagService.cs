namespace Application.Tweets.Shared.Service;

public interface IHashtagService
{
    Task<IList<HashtagVM>> GetHashTagAsync(string tagName);
    Task InsertHashtagInSearchTable(List<string> hashtags);
    Task<IList<HashtagVM>> GetTrendingHashtagAsync(int pageNumber);
    Task<IList<TweetViewModel>> GetHashtagTweetsAsync(string keyword, int pageNumber);
    List<string> ExtractHashTag(string content);
    Task InsertHashtag(List<string> hashtags, Tweet tweet);
    Task ProcessTweetsHashtag(Tweet tweet);
}