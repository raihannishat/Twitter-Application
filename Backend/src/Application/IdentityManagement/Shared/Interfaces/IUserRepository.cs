using Application.Common.Interfaces;
using MongoDB.Bson;

namespace Application.IdentityManagement.Shared.Interfaces;

public interface IUserRepository : IRepository<Domain.Entities.User>
{
    IEnumerable<Domain.Entities.User> GetUserNameByFullText(string name);
    IEnumerable<Domain.Entities.User> GetUserNameWithRegex(string name);
    IEnumerable<Domain.Entities.User> GetUserNameByFuzzySearch(string name);
}