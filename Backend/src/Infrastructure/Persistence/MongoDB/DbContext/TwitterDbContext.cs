namespace Infrastructure.Persistence.MongoDB.DbContext;

public class TwitterDbContext : ITwitterDbContext
{
    private IMongoDatabase _database { get; }
    private MongoClient _mongoClient { get; }

    public TwitterDbContext(IMongoDbSettings setting)
    {
        _mongoClient = new MongoClient(setting.ConnectionString);
        _database = _mongoClient.GetDatabase(setting.DatabaseName);
    }

    public IMongoCollection<TEntity> GetCollectionName<TEntity>(string name)
    {
        return _database.GetCollection<TEntity>(GetDocumentName<TEntity>(name));
    }

    private string GetDocumentName<TCollection>(string collectionName = "")
    {
        return string.IsNullOrWhiteSpace(collectionName) ?
            $"{typeof(TCollection).Name}" : collectionName;
    }
}
