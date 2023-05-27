namespace Application.Tweets.Shared.Interfaces;
public interface IHashtagRepository : IRepository<Hashtag>
{
    IEnumerable<Hashtag> GetHashtagByFullText(string name);
    IEnumerable<Hashtag> GetHashtagTweetByDescendingTime(Expression<Func<Hashtag, bool>> filterExpression, int pageNumber);

    //Task<IEnumerable<Hashtag>> GetHashtagWithPaginationAsync(int pageNumber);
}
