namespace Application.Tweets.Commands.DeleteTweet;

public class DeleteTweetCommand : IRequest<Result<Unit>>
{
	public string Id { get; set; } = string.Empty;

	public DeleteTweetCommand(string id) => Id = id;
}