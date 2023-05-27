using MongoDB.Driver;

namespace Infrastructure.Persistence.MongoDB.Repositories;

public class HashtagRepository : MongoRepository<Hashtag>, IHashtagRepository
{
    public HashtagRepository(ITwitterDbContext twitterDbContext) : base(twitterDbContext)
    {
    }
    public IEnumerable<Hashtag> GetHashtagByFullText(string name)
    {
        //var filter = Builders<Hashtag>.Filter.Where(x => x.TagName == name && x.Id == "43");

        //var res = _collection.Aggregate().Match(filter).ToEnumerable();


        // _collection.Indexes.CreateOne(new CreateIndexModel<Hashtag>(Builders<Hashtag>.IndexKeys.Text(x => x.Name)));

        var list = _collection.Find(Builders<Hashtag>.Filter.Text(name)).ToEnumerable();
        return list;
    }

    public IEnumerable<Hashtag> GetHashtagTweetByDescendingTime(Expression<Func<Hashtag, bool>> filterExpression, int pageNumber)
    {
        var filter = Builders<Hashtag>.Filter.Where(filterExpression);

        return _collection.Aggregate().Match(filter).SortByDescending(x => x.CreatedAt).Skip((pageNumber - 1) * 5).Limit(5).ToEnumerable();
    }

    //public async Task<IEnumerable<Hashtag>> GetHashtagWithPaginationAsync(int pageNumber)
    //{
    //    var filter = Builders<Hashtag>.Filter.Empty;

    //    var res = await _collection.Aggregate().Match(filter).Skip((pageNumber - 1) * 10).Limit(10).ToListAsync();

    //    return res;
    //}
}
