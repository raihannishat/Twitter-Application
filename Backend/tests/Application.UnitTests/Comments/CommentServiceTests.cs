namespace Application.UnitTests.Comments;

public class CommentServiceTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<ITweetRepository> _tweetRepositoryMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IBlockRepository> _blockRepositoryMock;
    private readonly CommentService _sut;

    public CommentServiceTests()
    {
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _tweetRepositoryMock = new Mock<ITweetRepository>();
        _notificationServiceMock = new Mock<INotificationService>();
        _commentRepositoryMock = new Mock<ICommentRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _blockRepositoryMock = new Mock<IBlockRepository>();

        _sut = new CommentService(_currentUserServiceMock.Object,
            _tweetRepositoryMock.Object,
            _commentRepositoryMock.Object,
            _userRepositoryMock.Object,
            _blockRepositoryMock.Object,
            _notificationServiceMock.Object);
    }

    [Fact]
    public async Task CreateCommentAsync_ShouldReturnNull_WhenInvalidTweetId()
    {
        // Arrange
        var tweetId = "988798B2-FA8C-42B9-A3BC-D8F177CED9B2";
        var content = "This is a comment";

        _tweetRepositoryMock.Setup(r => r.FindByIdAsync(tweetId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.CreateCommentAsync(tweetId, content);

        // Assert
        result.Should().BeNull();

        _commentRepositoryMock.Verify(x =>
            x.InsertOneAsync(It.IsAny<Comment>()), Times.Never());

        _tweetRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(It.IsAny<Tweet>()), Times.Never());

        _notificationServiceMock.Verify(x =>
            x.CreateNotificationAsync(It.IsAny<Tweet>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public async Task CreateCommentAsync_ShouldReturnCommentCount_WhenValidTweetIdAndContent()
    {
        // Arrange
        var tweetId = "6C68457A-1DD7-4EBF-B659-A4417F6E90AC";
        var content = "This is a comment";
        var userId = "5BC1434E-0E28-4D59-8901-6D29FF832079";
        var commentId = "269834EB-FA65-46E4-B556-19FB2E43455C";

        var comment = new Comment
        {
            Id = commentId,
            TweetId = tweetId,
            UserId = userId
        };

        var tweet = new Tweet
        {
            Id = tweetId,
            Comments = 0
        };

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(tweet);

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(userId);

        _commentRepositoryMock.Setup(x => x.InsertOneAsync(comment))
            .Verifiable();

        // Act
        var result = await _sut.CreateCommentAsync(tweetId, content);

        // Assert
        result.Should().Be(1);
        tweet.Comments.Should().Be(1);

        _commentRepositoryMock.Verify(x =>
            x.InsertOneAsync(It.IsAny<Comment>()), Times.Once());

        _tweetRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(tweet), Times.Once());

        _notificationServiceMock.Verify(x =>
            x.CreateNotificationAsync(tweet, "Commented"), Times.Once());
    }

    [Fact]
    public async Task DeleteCommentAsync_ShouldReturnNull_WhenInvalidTweetId()
    {
        // Arrange
        var tweetId = "16CDA007-D723-4DAB-8B87-8D07B5DC92AB";
        var commentId = "265F5B5A-4EBB-47F1-939F-B0ADB7B273AF";

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.DeleteCommentAsync(tweetId, commentId);

        // Assert
        result.Should().BeNull();

        _commentRepositoryMock.Verify(x =>
            x.DeleteByIdAsync(It.IsAny<string>()), Times.Never());

        _tweetRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(It.IsAny<Tweet>()), Times.Never());
    }

    [Fact]
    public async Task DeleteCommentAsync_ShouldReturnNull_WhenInvalidCommentUserId()
    {
        // Arrange
        var tweetId = "29E86F74-1B67-44A5-A698-7E5525BFEFA8";
        var commentId = "C15B87F1-DB03-432F-8EA5-9C0971EC7753";
        var currentUserId = "77432726-6176-41A6-AFE8-FD724B229BC1";
        var commentUserId = "8365C6FE-114F-4532-87AE-2990BBDAE930";

        var tweet = new Tweet
        {
            Id = tweetId,
            Comments = 1
        };

        var comment = new Comment
        {
            Id = commentId,
            TweetId = tweetId,
            UserId = commentUserId
        };

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(tweet);

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _commentRepositoryMock.Setup(x => x.FindByIdAsync(commentId))
            .ReturnsAsync(comment);

        // Act
        var result = await _sut.DeleteCommentAsync(tweetId, commentId);

        // Assert
        result.Should().BeNull();
        tweet.Comments.Should().Be(1);

        _commentRepositoryMock.Verify(x =>
            x.DeleteByIdAsync(It.IsAny<string>()), Times.Never());

        _tweetRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(It.IsAny<Tweet>()), Times.Never());
    }

    [Fact]
    public async Task DeleteCommentAsync_ShouldReturnCommentCount_WhenValidTweetIdAndCommentId()
    {
        // Arrange
        var tweetId = "DEACCA46-5122-47CA-891A-710949CCEB8C";
        var commentId = "CE4DE17A-9CD9-48D6-940C-536C701DA3F5";
        var userId = "B973915E-C0EE-49C5-B5BA-A49CB716A7B8";

        var tweet = new Tweet
        {
            Id = tweetId,
            Comments = 1
        };

        var comment = new Comment
        {
            Id = commentId,
            TweetId = tweetId,
            UserId = userId
        };

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId)).ReturnsAsync(tweet);

        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

        _commentRepositoryMock.Setup(x => x.FindByIdAsync(commentId))
            .ReturnsAsync(comment);

        // Act
        var result = await _sut.DeleteCommentAsync(tweetId, commentId);

        // Assert
        result.Should().Be(0);
        tweet.Comments.Should().Be(0);

        _commentRepositoryMock.Verify(x =>
            x.DeleteByIdAsync(commentId), Times.Once());

        _tweetRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(tweet), Times.Once());
    }

    [Fact]
    public async Task GetCommentsAsync_ShouldReturnNull_WhenTweetNotFound()
    {
        // Arrange
        var tweetId = "EDFEE352-50F3-49AC-B13F-BEEFB2294C4C";
        var pageNumber = 1;

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync((Tweet)null!);

        // Act
        var result = await _sut.GetCommentsAsync(tweetId, pageNumber);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCommentsAsync_ShouldReturnEmptyList_WhenNoCommentsExist()
    {
        // Arrange
        var tweetId = "71D20F53-13CF-48FB-9A4E-273495C58860";
        var pageNumber = 1;

        var commentExpression = new Comment
        {
            TweetId = tweetId
        };

        var commentResult = new List<Comment>();

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(new Tweet());

        _commentRepositoryMock.Setup(x => x.GetCommentByDescendingTime(It.Is<Expression<Func<Comment, bool>>>(y =>
            y.Compile()(commentExpression)), pageNumber))
            .ReturnsAsync(commentResult);

        // Act
        var result = await _sut.GetCommentsAsync(tweetId, pageNumber);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
