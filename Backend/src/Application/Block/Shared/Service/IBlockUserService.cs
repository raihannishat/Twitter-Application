namespace Application.Block.Shared.Service;

public interface IBlockUserService
{
    Task<IList<UserViewModel>> GetBlockedUsersAsync(int pageNumber);
    Task<bool?> BlockUserAsync(string userId);
    Task<bool> IsBlockAsync(string blockedId, string blockedById);
}
