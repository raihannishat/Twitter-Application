namespace Domain.Entities;

[BsonCollection("Hashtag")]
public class Hashtag : Entity
{
    public string TagName { get; set; } = string.Empty;
    public string TweetId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
