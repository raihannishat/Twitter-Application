namespace Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommand : IRequest<Result<CommentResponse>>
{
    public string TweetId { get; set; } = string.Empty;
    public string CommentId { get; set; } = string.Empty;
    public DeleteCommentCommand(string tweetId, string commentId)
    {
        TweetId = tweetId;
        CommentId = commentId;
    }
}
