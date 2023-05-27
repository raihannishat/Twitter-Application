namespace Infrastructure.Persistence.MongoDB.DbContext;

public interface ITwitterDbContext
{
    IMongoCollection<TEntity> GetCollectionName<TEntity>(string name = "");
}
