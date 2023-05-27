namespace Application.Follows.Shared.Service;

public interface IFollowUserService
{
    Task<(bool IsFollowing, int Followers)?> FollowUserAsync(string targetUserId);
    Task<IList<UserViewModel>> GetFollowersAsync(string userId, int pageNumber);
    Task<IList<UserViewModel>> GetFollowingAsync(string userId, int pageNumber);
    Task UnfollowUserAsync(Follow follow);
    Task<bool> IsFollowing(string followerId, string followedId);
}
