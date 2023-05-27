namespace Application.Follows.Shared.Service;

public class FollowUserService : IFollowUserService
{
    private readonly IFollowRepository _followRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IBlockRepository _blockRepository;
    private readonly ITimelineService _timelineService;

    public FollowUserService(IFollowRepository followRepository,
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IBlockRepository blockRepository,
        ITimelineService timelineService)
    {
        _followRepository = followRepository;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _blockRepository = blockRepository;
        _timelineService = timelineService;
    }

    public async Task<(bool IsFollowing, int Followers)?> FollowUserAsync(string targetUserId)
    {
        (bool IsFollowing, int Followers) result = (false, 0);

        var currentUserId = _currentUserService.UserId;
        var currentUser = await _userRepository.FindByIdAsync(currentUserId);
        var targetUser = await _userRepository.FindByIdAsync(targetUserId);

        if (currentUserId == targetUserId || targetUser == null)
        {
            return null!;
        }

        var followEntity = await _followRepository.FindOneByMatchAsync(x =>
                x.FollowerId == currentUserId && x.FollowedId == targetUserId);

        if (followEntity == null)
        {
            await FollowAsync(currentUserId, targetUserId);
            await IncrementFollowCountsAsync(currentUser, targetUser);
            await _timelineService.AddUserTweetsToTimeline(currentUser, targetUser);
            result.IsFollowing = true;
        }
        else
        {
            await UnfollowUserAsync(followEntity);
            await DecrementFollowCounts(currentUser, targetUser);
            await _timelineService.DeleteTweetFromUserHomeTimeline(currentUserId, targetUserId);
        }

        result.Followers = targetUser.Followers;
        return (result.IsFollowing, result.Followers);
    }

    public async Task<IList<UserViewModel>> GetFollowersAsync(string userId, int pageNumber)
    {
        var targetUser = await _userRepository.FindByIdAsync(userId);

        if (targetUser == null)
        {
            return null!;
        }

        var followerEntities = _followRepository.FindByMatchWithPagination(
            x => x.FollowedId == userId, pageNumber);

        if (!followerEntities.Any())
        {
            return null!;
        }

        var followerUsers = await GetFilteredFollowers(followerEntities);
        var followers = await GetFollowersViewModel(followerUsers);
        var followerList = await ProcessEntitiesForCurrentUser(followers);

        return followerList;
    }

    public async Task<IList<UserViewModel>> GetFollowingAsync(string userId, int pageNumber)
    {
        var targetUser = await _userRepository.FindByIdAsync(userId);

        if (targetUser == null)
        {
            return null!;
        }

        var followingObj = _followRepository.FindByMatchWithPagination(x =>
            x.FollowerId == userId, pageNumber);

        if (!followingObj.Any())
        {
            return null!;
        }

        var followingUsers = await GetFilteredFollowing(followingObj);
        var followings = await GetFollowingViewModel(followingUsers);
        var followingList = await ProcessEntitiesForCurrentUser(followings);

        return followingList;
    }

    public async Task UnfollowUserAsync(Follow follow)
    {
        await _followRepository.DeleteByIdAsync(follow.Id);
    }

    private async Task FollowAsync(string followerId, string followedId)
    {
        var followEntity = new Follow
        {
            FollowerId = followerId,
            FollowedId = followedId
        };

        await _followRepository.InsertOneAsync(followEntity);
    }

    private async Task IncrementFollowCountsAsync(User currentUser, User targetUser)
    {
        currentUser.Followings++;
        targetUser.Followers++;

        await _userRepository.ReplaceOneAsync(currentUser);
        await _userRepository.ReplaceOneAsync(targetUser);
    }

    private async Task DecrementFollowCounts(User currentUser, User targetUser)
    {
        currentUser.Followings--;
        targetUser.Followers--;

        await _userRepository.ReplaceOneAsync(currentUser);
        await _userRepository.ReplaceOneAsync(targetUser);
    }

    private async Task<List<Follow>> GetFilteredFollowers(IEnumerable<Follow> followersObj)
    {
        var followerUsers = new List<Follow>();

        foreach (var follower in followersObj)
        {
            var currentUserIsBlock = await _blockRepository.FindOneByMatchAsync(x =>
                x.BlockedId == _currentUserService.UserId && x.BlockedById == follower.FollowerId);

            var followedUserIdBlock = await _blockRepository.FindOneByMatchAsync(x =>
                x.BlockedId == follower.FollowerId && x.BlockedById == _currentUserService.UserId);

            if (currentUserIsBlock == null && followedUserIdBlock == null)
            {
                followerUsers.Add(follower);
            }
        }

        return followerUsers;
    }

    private async Task<List<Follow>> GetFilteredFollowing(IEnumerable<Follow> followingObj)
    {
        var followingUsers = new List<Follow>();

        foreach (var following in followingObj)
        {
            var currentUserIsBlock = await _blockRepository.FindOneByMatchAsync(x =>
                x.BlockedId == _currentUserService.UserId && x.BlockedById == following.FollowedId);

            var followedUserIdBlock = await _blockRepository.FindOneByMatchAsync(x =>
                x.BlockedId == following.FollowedId && x.BlockedById == _currentUserService.UserId);

            if (currentUserIsBlock == null && followedUserIdBlock == null)
            {
                followingUsers.Add(following);
            }
        }

        return followingUsers;
    }

    private async Task<IEnumerable<UserViewModel>> GetFollowersViewModel(List<Follow> followerUsers)
    {
        var userCollection = _userRepository.GetCollection();

        var followers = from follower in followerUsers
                        join user in userCollection on follower.FollowerId equals user.Id
                        select new UserViewModel
                        {
                            Id = user.Id,
                            Name = user.Name,
                            Image = user.Image
                        };

        return followers;
    }

    private async Task<IEnumerable<UserViewModel>> GetFollowingViewModel(List<Follow> followingUsers)
    {
        var userCollection = _userRepository.GetCollection();

        var followings = from follower in followingUsers
                         join user in userCollection on follower.FollowedId equals user.Id
                         select new UserViewModel
                         {
                             Id = user.Id,
                             Name = user.Name,
                             Image = user.Image
                         };

        return followings;
    }

    private async Task<List<UserViewModel>> ProcessEntitiesForCurrentUser(IEnumerable<UserViewModel> users)
    {
        var userEntities = new List<UserViewModel>();

        if (users.Any())
        {
            foreach (var user in users)
            {
                user.IsFollowing = await IsFollowing(_currentUserService.UserId, user.Id);

                if (user.Id == _currentUserService.UserId)
                {
                    user.IsCurrentUser = true;
                }
                userEntities.Add(user);
            }
        }

        return userEntities;
    }

    public async Task<bool> IsFollowing(string followerId, string followedId)
    {
        var followEntity = await _followRepository.FindOneByMatchAsync(x =>
            x.FollowerId == followerId && x.FollowedId == followedId);

        return followEntity != null;
    }
}
