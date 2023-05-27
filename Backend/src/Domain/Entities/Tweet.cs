namespace Domain.Entities;
[BsonCollection("Tweet")]
public class Tweet : Entity
{
    public string? UserId { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Edited { get; set; }
    public int Comments { get; set; }
    public int Likes { get; set; }
    public int Retweets { get; set; }
}
