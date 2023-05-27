using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Text.Json;

namespace Infrastructure.Persistence.MongoDB.Repositories;

public class UserRepository : MongoRepository<User>, IUserRepository
{
    public UserRepository(ITwitterDbContext context) : base(context)
    {

    }

    public IEnumerable<User> GetUserNameByFuzzySearch(string name)
    {
        BsonDocument[] pipeline = new BsonDocument[] {
            new BsonDocument("$search",
            new BsonDocument
                {
                    { "index", "name" },
                    { "autocomplete",
            new BsonDocument
                    {
                        { "query", name },
                        { "path", "Name" },
                        { "fuzzy",
            new BsonDocument
                        {
                            { "maxEdits", 1 },
                            { "prefixLength", 1 }
                        } }
                    } }
                }),
            new BsonDocument("$match",
            new BsonDocument("Email",
            new BsonDocument("$ne", "admin@gmail.com"))),
            new BsonDocument("$project",
            new BsonDocument
                {
                    { "_id", 1 },
                    { "Name", 1 },
                    { "Image", 1 }
                }),
            new BsonDocument("$limit", 10)
        };

        var results = _collection.Aggregate<BsonDocument>(pipeline).ToList();
        
        var users = new List<User>();
        
        foreach (var result in results)
        {
            var user = BsonSerializer.Deserialize<User>(result);
            
            users.Add(user);
        }
        
        return users;
    }

    public IEnumerable<User> GetUserNameByFullText(string name)
    {
        _collection.Indexes.CreateOne(new CreateIndexModel<User>(Builders<User>.IndexKeys.Text(x => x.Name)));

        //var list = _collection.Find(Builders<User>.Filter.Text(name)).Skip((pageNumber-1)*5).Limit(pageSize).ToEnumerable();
        
        var list = _collection.Find(Builders<User>.Filter.Text(name)).Limit(10).ToEnumerable();
        //var filter = Builders<User>.Filter.Regex(x => x.Name, "");
        return list;
    }

    public IEnumerable<User> GetUserNameWithRegex(string name)
    {
        var keyword = @"/^" + name + "| " + name + "/i";
        var filter = Builders<User>.Filter.Regex(x => x.Name, keyword);

        var res = _collection.Aggregate().Match(filter).Limit(10).ToEnumerable();

        return res;
    }
}
