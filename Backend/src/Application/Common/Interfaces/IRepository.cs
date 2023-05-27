namespace Application.Common.Interfaces;

public interface IRepository<TEntity> where TEntity : IEntity
{
    IEnumerable<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression);
    IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TEntity, bool>> filterExpression,
        Expression<Func<TEntity, TProjected>> projectionExpression);

    Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression);
    Task<TEntity> FindOneByMatchAsync(Expression<Func<TEntity, bool>> filterExpression);
    IEnumerable<TEntity> FindByMatchWithPagination(Expression<Func<TEntity, bool>> filterExpression, int pageNumber);
    Task<TEntity> FindByIdAsync(string id);
    Task InsertOneAsync(TEntity document);
    Task ReplaceOneAsync(TEntity document);
    Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression);
    //Task<IEnumerable<TEntity>> GetFilterByAsync(Expression<Func<TEntity, bool>> filterExpression);
    Task DeleteByIdAsync(string id);
    Task<List<TEntity>> GetAllWithPaginationAsync(PaginationQueryRequest query);
    Task<List<TEntity>> GetAllAsync();
    Task<long> CountAsync();

    IQueryable<TEntity> GetCollection();
}
