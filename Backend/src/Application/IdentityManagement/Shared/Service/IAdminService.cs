namespace Application.IdentityManagement.Shared.Service;

public interface IAdminService
{
    Task<bool?> BlockUserAsync(string userId);
    Task<IList<UserProfileViewModel>?> GetAllUsersAsync(int pageNumber);
}