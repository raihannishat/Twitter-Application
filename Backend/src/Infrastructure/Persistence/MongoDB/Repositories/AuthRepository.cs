using Infrastructure.Repositories;

namespace Infrastructure.Persistence.MongoDB.Repositories;

public class AuthRepository : MongoRepository<RefreshToken>, IAuthRepository
{
    public AuthRepository(ITwitterDbContext context) : base(context)
    {

    }
}
