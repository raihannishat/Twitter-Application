namespace Application.Likes.Shared.Service;

public class LikeService : ILikeService
{
    private readonly ILikeRepository _likeRepository;
    private readonly ITweetRepository _tweetRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;

    public LikeService(ILikeRepository likeRepository,
        ITweetRepository tweetRepository,
        ICurrentUserService currentUserService,
        INotificationService notificationService)
    {
        _likeRepository = likeRepository;
        _tweetRepository = tweetRepository;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
    }

    public async Task<(bool IsLikedByCurrentUser, int Likes)?> LikeAsync(string tweetId)
    {
        (bool IsLikedByCurrentUser, int Likes) result = (false, 0);

        var currentUserId = _currentUserService.UserId;
        var tweetEntity = await _tweetRepository.FindByIdAsync(tweetId);

        if (tweetEntity == null)
        {
            return null!;
        }

        var likeEntity = await _likeRepository.FindOneByMatchAsync(x =>
            x.UserId == _currentUserService.UserId && x.TweetId == tweetId);

        if (likeEntity == null)
        {
            await CreateLike(tweetEntity);
            await _notificationService.CreateNotificationAsync(tweetEntity, actionType: TweetConstants.LikeAction);
            tweetEntity.Likes++;
            result.IsLikedByCurrentUser = true;
        }
        else
        {
            await RemoveLike(tweetId, currentUserId);
            tweetEntity.Likes--;
        }

        await _tweetRepository.ReplaceOneAsync(tweetEntity);
        result.Likes = tweetEntity.Likes;
        return (result.IsLikedByCurrentUser, result.Likes);
    }

    public async Task RemoveLike(string tweetId, string userId)
    {
        await _likeRepository.DeleteOneAsync(x => x.TweetId == tweetId && x.UserId == userId);
    }

    public async Task<bool> IsTweetLikedByUser(string tweetId, string userId)
    {
        var like = await _likeRepository.FindOneByMatchAsync(x => x.TweetId == tweetId && x.UserId == userId);
        return like != null;
    }

    private async Task CreateLike(Tweet tweetEntity)
    {
        var likeEntity = new Like
        {
            UserId = _currentUserService.UserId,
            TweetId = tweetEntity.Id
        };

        await _likeRepository.InsertOneAsync(likeEntity);
    }
}
