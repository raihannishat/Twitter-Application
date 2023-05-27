using Application.Follows.Shared.Interfaces;

namespace Infrastructure.Persistence.MongoDB.Repositories;

public class FollowRepository : MongoRepository<Follow>, IFollowRepository
{
    public FollowRepository(ITwitterDbContext twitterDbContext) : base(twitterDbContext)
    {
    }

    public async Task<List<Follow>> GetFollowers(Expression<Func<Follow, bool>> filterExpression)
    {
        var filter = Builders<Follow>.Filter.Where(filterExpression);

        var res = await _collection.Aggregate().Match(filter).ToListAsync();

        return res;
    }

    public async Task<List<Follow>> GetFollowings(Expression<Func<Follow, bool>> filterExpression)
    {
        var filter = Builders<Follow>.Filter.Where(filterExpression);

        var res = await _collection.Aggregate().Match(filter).ToListAsync();

        return res;
    }
}
