namespace Application.Comments.Commands.CreateComment;

public class CreateCommentCommand : IRequest<Result<CommentResponse>>
{
    public string TweetId { get; set; } = string.Empty;
    public string TweetOwnerId { get; set; } = string.Empty;
    public string TweetCreatorId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
