using Application.Common.Interfaces;

namespace Application.Follows.Shared.Interfaces;

public interface IFollowRepository : IRepository<Follow>
{
    Task<List<Follow>> GetFollowers(Expression<Func<Follow, bool>> filterExpression);
    Task<List<Follow>> GetFollowings(Expression<Func<Follow, bool>> filterExpression);
}
