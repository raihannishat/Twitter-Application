namespace Application.Tweets.Shared.Models;

public class TweetViewModel
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string Image { get; set; } = string.Empty;
    public string? TweetCreatorId { get; set; }
    public string? TweetCreatorName { get; set; }
    public string? TweetCreatorImage { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Edited { get; set; }
    public bool CanDelete { get; set; }
    public int Likes { get; set; }
    public int Retweets { get; set; }
    public int Comments { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public bool IsRetweetedByCurrentUser { get; set; }
    public bool IsRetweeted { get; set; }
}
