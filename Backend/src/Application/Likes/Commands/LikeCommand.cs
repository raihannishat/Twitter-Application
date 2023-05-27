namespace Application.Likes.Commands;

public class LikeCommand : IRequest<Result<LikeResponse>>
{
    public string? TweetId { get; set; } = string.Empty;
    public string TweetOwnerId { get; set; } = string.Empty;
    public string TweetCreatorId { get; set; } = string.Empty;
}
