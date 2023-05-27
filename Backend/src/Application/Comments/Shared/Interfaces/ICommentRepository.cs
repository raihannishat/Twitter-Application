namespace Application.Comments.Shared.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetCommentByDescendingTime(Expression<Func<Comment, bool>> filterExpression, int pageNumber);
}
