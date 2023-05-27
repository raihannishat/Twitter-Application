using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Infrastructure.Persistence.MongoDB.Repositories;

public class SearchRepository : MongoRepository<Search>, ISearchRepository
{
    public SearchRepository(ITwitterDbContext twitterDbContext) : base(twitterDbContext)
    {
    }

    public IEnumerable<Search> GetHashtagByFuzzySearch(string name)
    {
        BsonDocument[] pipeline = new BsonDocument[] {
            new BsonDocument("$search",
            new BsonDocument
                {
                    { "index", "hashtag" },
                    { "autocomplete",
            new BsonDocument
                    {
                        { "query", name },
                        { "path", "HashTag" },
                        { "fuzzy",
            new BsonDocument
                        {
                            { "maxEdits", 1 },
                            { "prefixLength", 1 }
                        } }
                    } }
                }),
            new BsonDocument("$project",
            new BsonDocument("HashTag", 1)),
            new BsonDocument("$limit", 10)
        };

        var results = _collection.Aggregate<BsonDocument>(pipeline).ToList();

        var tags = new List<Search>();

        foreach (var result in results)
        {
            var tag = BsonSerializer.Deserialize<Search>(result);

            tags.Add(tag);
        }

        return tags;
    }

    public async Task<IEnumerable<Search>> GetHashtagWithPagination(int pageNumber)
    {
        var filter = Builders<Search>.Filter.Empty;

        var res = await _collection.Aggregate().Match(filter).Skip((pageNumber - 1) * 10).Limit(10).ToListAsync();

            return res;
    }

    public async Task<IEnumerable<Search>> GetHashtagByRegex(string name)
    {
        var keyword = @"/^" + name + @"/i";

        var filter = Builders<Search>.Filter.Regex(x => x.HashTag, keyword);

        var res = await _collection.Aggregate().Match(filter).Limit(10).ToListAsync();

        return res;
    }
}
