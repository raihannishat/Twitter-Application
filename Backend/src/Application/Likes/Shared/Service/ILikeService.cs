namespace Application.Likes.Shared.Service;

public interface ILikeService
{
    Task<(bool IsLikedByCurrentUser, int Likes)?> LikeAsync(string tweetId);
    Task RemoveLike(string tweetId, string userId);
    Task<bool> IsTweetLikedByUser(string tweetId, string userId);
}
