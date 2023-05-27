namespace Domain.Entities;

[BsonCollection("Like")]
public class Like : Entity
{
    public string TweetId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}
