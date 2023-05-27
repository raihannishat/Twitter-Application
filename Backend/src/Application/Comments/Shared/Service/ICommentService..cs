namespace Application.Comments.Shared.Service;

public interface ICommentService
{
    Task<int?> CreateCommentAsync(string tweetId, string content);
    Task<int?> DeleteCommentAsync(string tweetId, string commentId);
    Task<IList<CommentViewModel>> GetCommentsAsync(string tweetId, int pageNumber);
}
