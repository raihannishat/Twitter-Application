namespace Application.Block.Shared.Service;

public class BlockUserService : IBlockUserService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IBlockRepository _blockRepository;
    private readonly IUserRepository _userRepository;

    public BlockUserService(ICurrentUserService currentUserService,
        IUserRepository userRepository, IBlockRepository blockRepository)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _blockRepository = blockRepository;
    }

    public async Task<bool?> BlockUserAsync(string userId)
    {
        var targetUser = await _userRepository.FindByIdAsync(userId);

        var currentUserId = _currentUserService.UserId;

        if (targetUser == null || userId == currentUserId)
        {
            return null!;
        }

        var blockObj = await _blockRepository.FindOneByMatchAsync(x =>
            x.BlockedId == targetUser.Id && x.BlockedById == currentUserId);

        if (blockObj == null)
        {
            var blockEntity = new Blocks()
            {
                BlockedId = targetUser.Id,
                BlockedById = currentUserId
            };

            await _blockRepository.InsertOneAsync(blockEntity);

            return true;
        }
        else
        {
            await _blockRepository.DeleteByIdAsync(blockObj.Id);
        }

        return false;
    }

    public async Task<IList<UserViewModel>> GetBlockedUsersAsync(int pageNumber)
    {
        var currentUserId = _currentUserService.UserId;

        var blockedUsersObj = _blockRepository.FindByMatchWithPagination(x =>
            x.BlockedById == currentUserId, pageNumber);

        var blockedUserIds = blockedUsersObj.Select(x => x.BlockedId).ToList();

        var tasks = blockedUserIds.Select(x => GetUserViewModelAsync(x));

        var userViewModels = await Task.WhenAll(tasks);

        return userViewModels;
    }

    public async Task<bool> IsBlockAsync(string blockedId, string blockedById)
    {
        var blockedUser = await _blockRepository.FindOneByMatchAsync(x => x.BlockedId == blockedId && x.BlockedById == blockedById);
        return blockedUser != null;
    }

    private async Task<UserViewModel> GetUserViewModelAsync(string userId)
    {
        var user = await _userRepository.FindByIdAsync(userId);

        return new UserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Image = user.Image,
            IsBlocked = true
        };
    }
}
