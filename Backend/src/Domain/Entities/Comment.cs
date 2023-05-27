namespace Domain.Entities;

public class Comment : Entity
{
    public string TweetId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
}
