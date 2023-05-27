using Application.Block.Shared.Interfaces;

namespace Infrastructure.Persistence.MongoDB.Repositories;

public class BlockRepository : MongoRepository<Blocks>, IBlockRepository
{
    public BlockRepository(ITwitterDbContext twitterDbContext) : base(twitterDbContext)
    {
    }
}
