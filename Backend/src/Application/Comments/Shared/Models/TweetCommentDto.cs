namespace Application.Comments.Shared.Models;

public class TweetCommentDto
{
    public string Id { get; set; } = string.Empty;
    public List<string>? TweetComments { get; set; }
}
