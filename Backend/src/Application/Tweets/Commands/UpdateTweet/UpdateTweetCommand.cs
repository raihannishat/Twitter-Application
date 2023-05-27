namespace Application.Tweets.Commands.UpdateTweet;

public  class UpdateTweetCommand : IRequest<Result<Unit>>
{
    public string? Id { get; set; } = null!;
    public string? Content { get; set; } = null!;
}
