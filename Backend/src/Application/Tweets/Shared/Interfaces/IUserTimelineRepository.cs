namespace Application.Tweets.Shared.Interfaces;

public interface IUserTimelineRepository : IRepository<UserTimeline>
{
    IEnumerable<UserTimeline> GetTweetByDescendingTime(Expression<Func<UserTimeline, bool>> filterExpression, int pageNumber);
}
