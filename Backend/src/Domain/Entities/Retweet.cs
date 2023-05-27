namespace Domain.Entities;

[BsonCollection("ReTweet")]
public class Retweet : Entity
{
    public string TweetId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}
