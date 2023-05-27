namespace Domain.Entities;

public class UserTimeline : Entity
{
    public string UserId { get; set; } = string.Empty;
    public string TweetId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
