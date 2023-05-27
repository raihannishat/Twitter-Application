namespace Application.Tweets.Commands.CreateTweet;

public class CreateTweetCommand : IRequest<Result<Unit>>
{
    public string Content { get; set; } = null!;
}
