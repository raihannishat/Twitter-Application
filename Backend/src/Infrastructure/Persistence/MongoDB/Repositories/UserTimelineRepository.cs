namespace Infrastructure.Persistence.MongoDB.Repositories;

public class UserTimelineRepository : MongoRepository<UserTimeline>, IUserTimelineRepository
{
    public UserTimelineRepository(ITwitterDbContext twitterDbContext) : base(twitterDbContext)
    {
    }

    public IEnumerable<UserTimeline> GetTweetByDescendingTime(Expression<Func<UserTimeline, bool>> filterExpression, int pageNumber)
    {
        var filter = Builders<UserTimeline>.Filter.Where(filterExpression);

        return _collection.Aggregate().Match(filter).SortByDescending(x => x.CreatedAt).Skip((pageNumber - 1) * 5).Limit(5).ToEnumerable();
    }
}
