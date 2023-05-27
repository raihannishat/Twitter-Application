namespace Infrastructure.Persistence.MongoDB.Repositories;

public class TweetRepository : MongoRepository<Tweet>, ITweetRepository
{
    public TweetRepository(ITwitterDbContext context) : base(context)
    {
       
    }

    //public async Task DeleteTweetCommentAsync(string tweetId, string commentId)
    //{
    //    var update = Builders<Tweet>.Update.PullFilter(x =>
    //        x.Comments, Builders<Comment>.Filter.Eq(x => x.Id, commentId));

    //    await _collection.UpdateOneAsync(x => x.Id.Equals(tweetId), update);
    //}

    public async Task<IEnumerable<Tweet>> GetTweetByHashtag(string hashtag, int pageNumber, int pageSize)
    {
        _collection.Indexes.CreateOne(new CreateIndexModel<Tweet>(Builders<Tweet>.IndexKeys.Text(x => x.Content)));

        var list = _collection.Find(Builders<Tweet>.Filter.Text(hashtag)).Skip((pageNumber-1)*5).Limit(pageSize).ToEnumerable();

        return list;
    }

    public IEnumerable<Tweet> GetUserTweet(string userId)
    {
        var filter = Builders<Tweet>.Filter.Where(x => x.UserId == userId);

        return _collection.Aggregate().Match(filter).SortByDescending(x => x.CreatedAt).Limit(5).ToEnumerable();
    }
}