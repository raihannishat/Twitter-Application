namespace Application.Tweets.Shared.Models;

public class TweetDto
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Edited { get; set; }
    public int Likes { get; set; }
    public int Comment { get; set; }
    public int Retweets { get; set; }
}
