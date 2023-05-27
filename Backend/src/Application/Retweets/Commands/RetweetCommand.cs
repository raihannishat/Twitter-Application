namespace Application.Retweets.Commands;

public class RetweetCommand : IRequest<Result<RetweetResponse>>
{
    public string? TweetId { get; set; } = null!;
    public string TweetCreatorId { get; set; } = string.Empty;
    public string TweetOwnerId { get; set; } = string.Empty;
}
