namespace Domain.Entities;

public class HomeTimeline : Entity
{
    public string UserId { get; set; } = string.Empty;
    public string TweetId { get; set; } = string.Empty;
    public string TweetOwnerId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
