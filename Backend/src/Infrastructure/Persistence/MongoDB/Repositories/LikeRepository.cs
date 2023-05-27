using Application.Likes.Shared.Interfaces;

namespace Infrastructure.Persistence.MongoDB.Repositories;

public class LikeRepository : MongoRepository<Like>, ILikeRepository
{
    public LikeRepository(ITwitterDbContext twitterDbContext) : base(twitterDbContext)
    {
    }
}
