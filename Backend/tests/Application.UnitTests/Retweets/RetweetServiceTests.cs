namespace Application.UnitTests.Retweets;

public class RetweetServiceTests
{
    private readonly Mock<IRetweetRepository> _retweetRepositoryMock;
    private readonly Mock<IUserTimelineRepository> _userTimelineRepositoryMock;
    private readonly Mock<ITweetRepository> _tweetRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly RetweetService _sut;

    public RetweetServiceTests()
    {
        _retweetRepositoryMock = new();
        _userTimelineRepositoryMock = new();
        _tweetRepositoryMock = new();
        _currentUserServiceMock = new();
        _notificationServiceMock = new();

        _sut = new RetweetService(_retweetRepositoryMock.Object,
            _userTimelineRepositoryMock.Object,
            _tweetRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _notificationServiceMock.Object);
    }

    [Fact]
    public async Task RetweetAsync_ShouldBeNull_WhenTweetDoseNotExist()
    {
        // Arrange
        var tweetId = "3AB084E0-1610-45FB-A964-B1BC0576B236";
        var currentUserId = "3D00DB3C-7BD7-4370-BF60-4E8EFB639F20";

        var tweet = new Tweet
        {
            Id = tweetId
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.RetweetAsync(tweetId);

        // Assert
        result.Should().BeNull();

        _tweetRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(tweet), Times.Never());
    }

    [Fact]
    public async Task RetweetAsync_ShouldBeRetweet_WhenTweetIsExist()
    {
        // Arrange
        var tweetId = "3AB084E0-1610-45FB-A964-B1BC0576B236";
        var currentUserId = "3D00DB3C-7BD7-4370-BF60-4E8EFB639F20";


        var tweet = new Tweet
        {
            Id = tweetId
        };

        var retweetEntity = new Retweet
        {
            TweetId = tweetId,
            UserId = currentUserId
        };

        var retweetResult = new Retweet();

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(tweet);

        // Act
        var result = await _sut.RetweetAsync(tweetId);

        // Assert
        result.Value.Retweets.Should().Be(1);

        _tweetRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(tweet), Times.Once());
    }

    [Fact]
    public async Task RetweetAsync_ShouldBeRemove_WhenTweetIsRetweeted()
    {
        // Arrange
        var tweetId = "90A6CD92-2055-4788-8696-93156E399C44";
        var currentUserId = "D71CA6B3-556A-4E84-87E9-F8E6E6F2A450";

        var tweet = new Tweet
        {
            Id = tweetId
        };

        var retweetEntity = new Retweet
        {
            TweetId = tweetId,
            UserId = currentUserId
        };

        var retweetResult = new Retweet();

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(tweet);

        _retweetRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Retweet, bool>>>(y =>
            y.Compile()(retweetEntity))))
            .ReturnsAsync(retweetResult);

        // Act
        var result = await _sut.RetweetAsync(tweetId);

        // Assert
        result.Value.Retweets.Should().Be(-1);

        _tweetRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(tweet), Times.Once());
    }

    [Fact]
    public async Task IsRetweetedByUser_ShouldBeTrue_WhenReTweetIsExist()
    {
        // Arrange
        var tweetId = "9238D832-914D-4883-A743-1EFE3C4F932E";
        var currentUserId = "02039EEE-D237-45DA-8ADB-81D004DA448E";

        var tweet = new Tweet
        {
            Id = tweetId
        };

        var retweetEntity = new Retweet
        {
            TweetId = tweetId,
            UserId = currentUserId
        };

        var retweetResult = new Retweet();

        _retweetRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Retweet, bool>>>(y =>
            y.Compile()(retweetEntity))))
            .ReturnsAsync(retweetResult);

        // Act
        var result = await _sut.IsRetweetedByUser(tweetId, currentUserId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsRetweetedByUser_ShouldBeFalse_WhenReTweetDoseNotExist()
    {
        // Arrange
        var tweetId = "9238D832-914D-4883-A743-1EFE3C4F932E";
        var currentUserId = "02039EEE-D237-45DA-8ADB-81D004DA448E";

        var tweet = new Tweet
        {
            Id = tweetId
        };

        var retweetEntity = new Retweet
        {
            TweetId = tweetId,
            UserId = currentUserId
        };

        _retweetRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Retweet, bool>>>(y =>
            y.Compile()(retweetEntity))))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.IsRetweetedByUser(tweetId, currentUserId);

        // Assert
        result.Should().BeFalse();
    }
}
