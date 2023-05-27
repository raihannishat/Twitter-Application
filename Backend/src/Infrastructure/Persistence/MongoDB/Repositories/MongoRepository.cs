namespace Infrastructure.Repositories;

public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
{
    private readonly ITwitterDbContext _twitterDbContext;
    protected readonly IMongoCollection<TEntity> _collection;

    public MongoRepository(ITwitterDbContext twitterDbContext)
    {
        _twitterDbContext = twitterDbContext;
        _collection = _twitterDbContext.GetCollectionName<TEntity>();
    }


    public async Task<TEntity> FindByIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, id);

        return await _collection
            .Find(filter)
            .SingleOrDefaultAsync();
    }

    public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        return await _collection
            .Find(filterExpression)
            .FirstOrDefaultAsync();
    }

    public async Task<List<TEntity>> GetAllWithPaginationAsync(PaginationQueryRequest query)
    {
        return await _collection
            .Find(_ => true)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Limit(query.PageSize)
            .ToListAsync();
    }

    public async Task InsertOneAsync(TEntity document)
    {
        await _collection.InsertOneAsync(document);
    }

    public async Task ReplaceOneAsync(TEntity document)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);

        await _collection.FindOneAndReplaceAsync(filter, document);
    }

    public async Task DeleteByIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, id);

        await _collection.FindOneAndDeleteAsync(filter);
    }

    public async Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        await _collection.FindOneAndDeleteAsync(filterExpression);
    }

    public async Task<long> CountAsync()
    {
        return await _collection.CountDocumentsAsync(_ => true);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public IQueryable<TEntity> GetCollection()
    {
        return _collection.AsQueryable();
    }

    public IEnumerable<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).ToEnumerable();
    }

    public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression)
    {
        return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
    }

    public IEnumerable<TEntity> FindByMatchWithPagination(Expression<Func<TEntity, bool>> filterExpression, int pageNumber)
    {
        var filter = Builders<TEntity>.Filter.Where(filterExpression);

        return _collection.Aggregate().Match(filter).Skip((pageNumber - 1) * 10).Limit(10).ToEnumerable();
    }

    public async Task<TEntity> FindOneByMatchAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        var filter = Builders<TEntity>.Filter.Where(filterExpression);

        return await _collection.Aggregate().Match(filter).FirstOrDefaultAsync();
    }
}

