using Application.Comments.Shared.Interfaces;

namespace Infrastructure.Persistence.MongoDB.Repositories;

public class CommentRepository : MongoRepository<Comment>, ICommentRepository
{
    public CommentRepository(ITwitterDbContext twitterDbContext) : base(twitterDbContext)
    {
    }

    public async Task<IEnumerable<Comment>> GetCommentByDescendingTime(Expression<Func<Comment, bool>> filterExpression, int pageNumber)
    {
        var filter = Builders<Comment>.Filter.Where(filterExpression);

        var res = await _collection.Aggregate().Match(filter).SortByDescending(x => 
            x.CreatedTime).Skip((pageNumber - 1)* 5).Limit(5).ToListAsync();

        return res;
    }
}
