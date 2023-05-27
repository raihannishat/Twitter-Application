namespace Application.Comments.Shared.Service;

public class CommentService : ICommentService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ITweetRepository _tweetRepository;
    private readonly INotificationService _notificationService;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBlockRepository _blockRepository;

    public CommentService(ICurrentUserService currentUserService,
            ITweetRepository tweetRepository,
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IBlockRepository blockRepository,
            INotificationService notificationService)
    {
        _currentUserService = currentUserService;
        _tweetRepository = tweetRepository;
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _blockRepository = blockRepository;
        _notificationService = notificationService;
    }

    public async Task<int?> CreateCommentAsync(string tweetId, string content)
    {
        var tweet = await _tweetRepository.FindByIdAsync(tweetId);

        if (tweet == null)
        {
            return null!;
        }

        var comment = new Comment
        {
            TweetId = tweet.Id,
            UserId = _currentUserService.UserId,
            Content = content,
            CreatedTime = DateTime.UtcNow,
        };

        await _commentRepository.InsertOneAsync(comment);
        tweet.Comments++;
        await _tweetRepository.ReplaceOneAsync(tweet);
        await _notificationService.CreateNotificationAsync(tweet, TweetConstants.CommentAction);
        return tweet.Comments;
    }

    public async Task<int?> DeleteCommentAsync(string tweetId, string commentId)
    {
        var tweet = await _tweetRepository.FindByIdAsync(tweetId);

        if (tweet == null)
        {
            return null!;
        }

        var comment = await _commentRepository.FindByIdAsync(commentId);

        if (comment.UserId != _currentUserService.UserId)
        {
            return null!;
        }

        await _commentRepository.DeleteByIdAsync(comment.Id);

        tweet.Comments--;

        await _tweetRepository.ReplaceOneAsync(tweet);

        return tweet.Comments;
    }

    public async Task<IList<CommentViewModel>> GetCommentsAsync(string tweetId, int pageNumber)
    {
        var tweet = await _tweetRepository.FindByIdAsync(tweetId);

        if (tweet == null)
        {
            return null!;
        }

        var commentEntities = await _commentRepository.GetCommentByDescendingTime(x =>
            x.TweetId == tweetId, pageNumber);

        if (!commentEntities.Any())
        {
            return new List<CommentViewModel>();
        }

        var comments = await FilterCommentsByBlockedList(commentEntities);

        var commentViewModels = await CreateCommentViewModels(comments);

        return commentViewModels;
    }

    private async Task<IList<Comment>> FilterCommentsByBlockedList(IEnumerable<Comment> commentObj)
    {
        var comments = new List<Comment>();

        foreach (var comment in commentObj)
        {
            var currentUserIsBlocked = await _blockRepository.FindOneByMatchAsync(x =>
                x.BlockedId == _currentUserService.UserId && x.BlockedById == comment.UserId);

            var userIsBlocked = await _blockRepository.FindOneByMatchAsync(x =>
                x.BlockedById == _currentUserService.UserId && x.BlockedId == comment.UserId);

            if (currentUserIsBlocked == null && userIsBlocked == null)
            {
                comments.Add(comment);
            }
        }

        return comments;
    }

    private async Task<IList<CommentViewModel>> CreateCommentViewModels(IEnumerable<Comment> comments)
    {
        var userCollection = _userRepository.GetCollection();

        var commentCollection = from comment in comments
                                join
                                user in userCollection on comment.UserId equals user.Id
                                select new CommentViewModel
                                {
                                    Id = comment.Id,
                                    Content = comment.Content,
                                    TweetId = comment.TweetId,
                                    UserId = user.Id,
                                    UserName = user.Name,
                                    Image = user.Image,
                                    CreatedTime = comment.CreatedTime
                                };

        var commentList = new List<CommentViewModel>();

        foreach (var comment in commentCollection)
        {
            if (comment.UserId == _currentUserService.UserId)
            {
                comment.CanDelete = true;
            }

            commentList.Add(comment);
        }

        return commentList;
    }
}
