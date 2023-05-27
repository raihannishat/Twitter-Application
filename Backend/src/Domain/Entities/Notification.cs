namespace Domain.Entities;

public class Notification : Entity
{
    public string TweetId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string ActionedUserId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string TweetType { get; set; } = string.Empty;
    public DateTime Time { get; set; }
}
