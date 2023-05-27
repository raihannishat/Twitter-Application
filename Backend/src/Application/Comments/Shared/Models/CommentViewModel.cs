namespace Application.Comments.Shared.Models;

public class CommentViewModel
{
    public string? Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedTime { get; set; }
    public string TweetId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string Image { get; set; } = string.Empty;
    public bool CanDelete { get; set; } 
}
