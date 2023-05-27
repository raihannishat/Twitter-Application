using Application.Common.Interfaces;

namespace Application.IdentityManagement.Shared.Interfaces;

public interface IAuthRepository : IRepository<RefreshToken>
{

}