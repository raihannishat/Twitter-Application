namespace Application.UnitTests.Like;

public class LikeServiceTests
{
    private readonly Mock<ILikeRepository> _likeRepositoryMock;
    private readonly Mock<ITweetRepository> _tweetRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly LikeService _sut;

    public LikeServiceTests()
    {
        _likeRepositoryMock = new();
        _tweetRepositoryMock = new();
        _currentUserServiceMock = new();
        _notificationServiceMock = new();

        _sut = new LikeService(_likeRepositoryMock.Object,
            _tweetRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _notificationServiceMock.Object);
    }

    [Fact]
    public async Task LikeAsync_ShouldAddLike_WhenLikeDoesNotExist()
    {
        // Arrange
        var tweetId = "17BAA634-1042-4E28-AA5B-9BDFB3E2CB32";
        var currentUserId = "A63CECA2-88B7-4543-8C53-D691B262FCEA";

        var tweet = new Tweet
        {
            Id = tweetId
        };

        var likeExpression = new Domain.Entities.Like
        {
            UserId = currentUserId,
            TweetId = tweetId
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(tweet);

        _likeRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Domain.Entities.Like, bool>>>(y =>
            y.Compile()(likeExpression))))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.LikeAsync(tweetId);

        // Assert
        result.Value.IsLikedByCurrentUser.Should().BeTrue();
        result.Value.Likes.Should().Be(1);

        _notificationServiceMock.Verify(x =>
            x.CreateNotificationAsync(tweet, "Liked"), Times.Once());
    }

    [Fact]
    public async Task LikeAsync_ShouldRemoveLike_WhenLikeIsExist()
    {
        // Arrange
        var tweetId = "F66805A5-68E4-492D-967E-164ECA5200D0";
        var currentUserId = "A9425929-9521-495C-9CC6-A56EE26AC092";

        var tweet = new Tweet
        {
            Id = tweetId
        };

        var likeExpression = new Domain.Entities.Like
        {
            UserId = currentUserId,
            TweetId = tweetId
        };

        var likeResult = new Domain.Entities.Like();

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(tweet);

        _likeRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Domain.Entities.Like, bool>>>(y =>
            y.Compile()(likeExpression))))
            .ReturnsAsync(likeResult);

        // Act
        var result = await _sut.LikeAsync(tweetId);

        // Assert
        result.Value.IsLikedByCurrentUser.Should().BeFalse();
        result.Value.Likes.Should().Be(-1);

        _tweetRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(tweet), Times.Once());

        _notificationServiceMock.Verify(x =>
            x.CreateNotificationAsync(tweet, "Liked"), Times.Never());
    }

    [Fact]
    public async Task LikeAsync_ShouldBeNull_WhenTweetDoesNotExist()
    {
        // Arrange
        var tweetId = "17BAA634-1042-4E28-AA5B-9BDFB3E2CB32";
        var currentUserId = "A63CECA2-88B7-4543-8C53-D691B262FCEA";

        var likeExpression = new Domain.Entities.Like
        {
            UserId = currentUserId,
            TweetId = tweetId
        };

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.LikeAsync(tweetId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RemoveLike_ShouldRemoveLike_WhenTweetAndUserAreExist()
    {
        // Arrange
        var tweetId = "3A6A93AD-1402-47DE-958F-388EC04E9197";
        var userId = "C39DE7E4-019E-4453-B280-EA60EEB3C794";

        var likeExpression = new Domain.Entities.Like
        {
            TweetId = tweetId,
            UserId = userId
        };

        // Act
        await _sut.RemoveLike(tweetId, userId);

        // Assert
        _likeRepositoryMock.Verify(x =>
            x.DeleteOneAsync(It.Is<Expression<Func<Domain.Entities.Like, bool>>>(y =>
            y.Compile()(likeExpression))), Times.Once());
    }

    [Fact]
    public async Task IsTweetLikedByUser_ShouldBeTrue_WhenLikeIsExist()
    {
        // Arrange
        var tweetId = "3F352F16-9956-4B7D-BEBE-685245F1BC1D";
        var userId = "ED5BFB0A-7AB1-4E48-86E2-25B7904C9053";

        var likeExpression = new Domain.Entities.Like
        {
            TweetId = tweetId,
            UserId = userId
        };

        var likeResult = new Domain.Entities.Like();

        _likeRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Domain.Entities.Like, bool>>>(y =>
            y.Compile()(likeExpression))))
            .ReturnsAsync(likeResult);

        // Act
        var result = await _sut.IsTweetLikedByUser(tweetId, userId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsTweetLikedByUser_ShouldBeFalse_WhenLikeDoseNotExist()
    {
        // Arrange
        var tweetId = "3048E02C-3896-4791-A1F5-E319C9E3B3D9";
        var userId = "AFC12176-5BF7-465E-9B00-248F6F47F633";

        var likeExpression = new Domain.Entities.Like
        {
            TweetId = tweetId,
            UserId = userId
        };

        _likeRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Domain.Entities.Like, bool>>>(y =>
            y.Compile()(likeExpression))))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.IsTweetLikedByUser(tweetId, userId);

        // Assert
        result.Should().BeFalse();
    }
}
