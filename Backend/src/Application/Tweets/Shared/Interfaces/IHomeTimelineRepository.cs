namespace Application.Tweets.Shared.Interfaces;

public interface IHomeTimelineRepository : IRepository<HomeTimeline>
{
    Task DeleteUserHomeTimeline(Expression<Func<HomeTimeline, bool>> filterExpression);
    IEnumerable<HomeTimeline> GetTweetByDescendingTime(Expression<Func<HomeTimeline, bool>> filterExpression, int pageNumber);

}
