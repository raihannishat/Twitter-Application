using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IdentityManagement.User.Shared.Services;
public interface IUserService
{
    Task<UserProfileDto> GetUserByIdAsync(string userId);
    Task<List<UserViewModel>> GetAllUsersAsync(int pageNumber);
    Task<List<UserViewModel>> GetUsersByNameAsync(string name);
}
