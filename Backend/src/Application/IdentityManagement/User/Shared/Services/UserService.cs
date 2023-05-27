using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IdentityManagement.User.Shared.Services;
public class UserService : IUserService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IFollowUserService _followService;
    private readonly IBlockUserService _blockService;
    private readonly IMapper _mapper;
    public UserService(ICurrentUserService currentUserService, IUserRepository userRepository, IMapper mapper, IFollowUserService followService, IBlockUserService blockService)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _mapper = mapper;
        _followService = followService;
        _blockService = blockService;
    }

    public async Task<UserProfileDto> GetUserByIdAsync(string userId)
    {
        var user = await _userRepository.FindByIdAsync(userId);

        if (user == null)
        {
            return null!;
        }

        var userProfile = _mapper.Map<UserProfileDto>(user);

        if (userProfile.Id == _currentUserService.UserId)
        {
            userProfile.IsCurrentUserProfile = true;

            return userProfile;
        }

        userProfile.IsFollowing = await _followService.IsFollowing(_currentUserService.UserId, user.Id);

        userProfile.IsBlockedByCurrentUser = await _blockService.IsBlockAsync(user.Id, _currentUserService.UserId);

        return userProfile;
    }

    public async Task<List<UserViewModel>> GetAllUsersAsync(int pageNumber)
    {
        var currentUser = _currentUserService.UserId;

        var userList = _userRepository.FindByMatchWithPagination(x =>
            x.Id != currentUser && x.Email != "admin@gmail.com", pageNumber); ;

        if (!userList.Any())
        {
            return new List<UserViewModel>();
        }

        var users = await GetFilteredUsers(userList.ToList());
        
        var userViewModels = await MaptoUserViewModel(users);
        
        return userViewModels;

    }

    private async Task<List<UserViewModel>> MaptoUserViewModel(List<Domain.Entities.User> users)
    {
        var userViewModels = new List<UserViewModel>();

        foreach (var user in users)
        {

            var userEntity = new UserViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Image = user.Image,
                IsCurrentUser = false,
                IsBlocked = false
            };

            userEntity.IsFollowing = await _followService.IsFollowing(_currentUserService.UserId, user.Id);

            userViewModels.Add(userEntity);
        }
        
        return userViewModels;
    }

    private async Task<List<Domain.Entities.User>> GetFilteredUsers(List<Domain.Entities.User> userList)
    {
        var users = new List<Domain.Entities.User>();
        
        foreach (var user in userList)
        {
            var currentUserIsBlocked = await _blockService.IsBlockAsync(_currentUserService.UserId, user.Id);
            
            var userIsBlocked = await _blockService.IsBlockAsync(user.Id, _currentUserService.UserId);

            if (!currentUserIsBlocked && !userIsBlocked)
            {
                users.Add(user);
            }
        }
        
        return users;
    }

    public async Task<List<UserViewModel>> GetUsersByNameAsync(string name)
    {
        //var userList = _userRepository.GetUserNameByFuzzySearch(request.Name);

        var userList = _userRepository.GetUserNameWithRegex(name);
        
        if (!userList.Any())
        {
            return new List<UserViewModel>();
        }

        var users = new List<UserViewModel>();

        foreach (var user in userList)
        {
            var userEntity = _mapper.Map<UserViewModel>(user);

            var currentUserIsBlocked = await _blockService.IsBlockAsync(_currentUserService.UserId, user.Id);

            var userIsBlocked = await _blockService.IsBlockAsync(user.Id, _currentUserService.UserId);

            if (!currentUserIsBlocked  && !userIsBlocked && (user.Email != "admin@gmail.com"))
            {
                users.Add(userEntity);
            }
        }
        return users;
    }
}
