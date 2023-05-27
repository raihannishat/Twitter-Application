namespace Application.IdentityManagement.Shared.Service;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AdminService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<bool?> BlockUserAsync(string userId)
    {
        var entity = await _userRepository.FindByIdAsync(userId);

        if (entity == null)
        {
            return null!;
        }

        entity.IsBlockedByAdmin = !entity.IsBlockedByAdmin;

        await _userRepository.ReplaceOneAsync(entity);

        return entity.IsBlockedByAdmin;
    }

    public async Task<IList<UserProfileViewModel>?> GetAllUsersAsync(int pageNumber)
    {
        var userList = _userRepository.FindByMatchWithPagination(x =>
            x.Email != "admin@gmail.com", pageNumber); ;

        var users = new List<UserProfileViewModel>();

        foreach (var user in userList)
        {
            users.Add(_mapper.Map<UserProfileViewModel>(user));
        }

        return users;
    }
}
