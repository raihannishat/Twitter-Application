namespace Infrastructure.Persistence.MongoDB.Common;

public interface IMongoDbSettings
{
    string DatabaseName { get; set; }
    string ConnectionString { get; set; }
}