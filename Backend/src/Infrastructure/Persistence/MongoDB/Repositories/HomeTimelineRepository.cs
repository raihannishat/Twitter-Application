namespace Infrastructure.Persistence.MongoDB.Repositories;

public class HomeTimelineRepository : MongoRepository<HomeTimeline>, IHomeTimelineRepository
{
    public HomeTimelineRepository(ITwitterDbContext twitterDbContext) : base(twitterDbContext)
    {
    }

    public async Task DeleteUserHomeTimeline(Expression<Func<HomeTimeline, bool>> filterExpression)
    {
        var res = await _collection.DeleteManyAsync(filterExpression);
    }

    public IEnumerable<HomeTimeline> GetTweetByDescendingTime(Expression<Func<HomeTimeline, bool>> filterExpression, int pageNumber)
    {
        var filter = Builders<HomeTimeline>.Filter.Where(filterExpression);

        return _collection.Aggregate().Match(filter).SortByDescending(x => x.CreatedAt).Skip((pageNumber - 1) * 5).Limit(5).ToEnumerable();
    }
}
