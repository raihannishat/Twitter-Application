namespace Application.UnitTests.Tweets;

public class TweetServiceTests
{
    private readonly Mock<ITweetRepository> _tweetRepositoryMock;
    private readonly Mock<IUserTimelineRepository> _userTimelineRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILikeService> _likeServiceMock;
    private readonly Mock<IRetweetService> _retweetServiceMock;
    private readonly TweetService _sut;

    public TweetServiceTests()
    {
        _tweetRepositoryMock = new Mock<ITweetRepository>();
        _userTimelineRepositoryMock = new Mock<IUserTimelineRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _likeServiceMock = new Mock<ILikeService>();
        _retweetServiceMock = new Mock<IRetweetService>();

        _sut = new TweetService(
            _tweetRepositoryMock.Object,
            _userTimelineRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _userRepositoryMock.Object,
            _likeServiceMock.Object,
            _retweetServiceMock.Object);
    }

    [Fact]
    public async Task DeleteTweetAsync_ShouldBeNull_WhenTweetDoseNotExist()
    {
        // Arange
        var tweetId = "0D9487A5-A0A8-4A4E-9275-D5D7652E5665";

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.DeleteTweetAsync(tweetId);

        // Assert
        result.Should().BeNull();

        _tweetRepositoryMock.Verify(x =>
            x.DeleteByIdAsync(tweetId), Times.Never());

        _userTimelineRepositoryMock.Verify(x
            => x.DeleteOneAsync(It.IsAny<Expression<Func<UserTimeline, bool>>>()), Times.Never());
    }

    [Fact]
    public async Task DeleteTweetAsync_ShouldBeNull_WhenTweetOwnerAndUserAreNotSame()
    {
        // Arange
        var tweetId = "FFC3282D-18A8-4637-8A76-40C4EA97482B";
        var tweetOwnerId = "A7E54696-F2A9-4F68-9BF1-03D51A9D2F8B";
        var userId = "7241E830-45FD-4A26-8AD2-2B1E32649C36";

        var tweet = new Tweet
        {
            UserId = tweetOwnerId
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(userId);

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(tweet);

        // Act
        var result = await _sut.DeleteTweetAsync(tweetId);

        // Assert
        result.Should().BeNull();

        _tweetRepositoryMock.Verify(x =>
            x.DeleteByIdAsync(tweetId), Times.Never());

        _userTimelineRepositoryMock.Verify(x =>
            x.DeleteOneAsync(It.IsAny<Expression<Func<UserTimeline, bool>>>()), Times.Never());
    }

    [Fact]
    public async Task DeleteTweetAsync_ShouldBeTrue_WhenTweetIsExistAndTweetOwnerAndUserAreSame()
    {
        // Arrange
        var tweetId = "CC44C9A3-ACC5-49E8-9DB6-D98DAD7F8A07";
        var userId = "8453A249-177D-4E81-9CE9-7F6300AB9DB4";

        var tweet = new Tweet
        {
            Id = tweetId,
            UserId = userId
        };

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(tweet);

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(userId);

        // Act
        var result = await _sut.DeleteTweetAsync(tweetId);

        // Assert
        result.Should().BeTrue();

        _tweetRepositoryMock.Verify(x =>
            x.DeleteByIdAsync(tweetId), Times.Once());

        _userTimelineRepositoryMock.Verify(x =>
            x.DeleteOneAsync(It.IsAny<Expression<Func<UserTimeline, bool>>>()), Times.Once());
    }

    [Fact]
    public async Task GetTweetByIdAsync_ShouldBeNull_WhenTweetDoseNotExist()
    {
        // Arrange
        var tweetId = "38EDAD2F-1724-4DAC-B915-44F31750BE89";
        var tweetOwnerId = "6EE4EBF2-DED4-4EA7-B37E-C45CEAE98BCE";

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.GetTweetByIdAsync(tweetId, tweetOwnerId);

        // Assert
        result.Should().BeNull();

        _userRepositoryMock.Verify(x =>
            x.FindByIdAsync(It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public async Task GetTweetByIdAsync_ShouldBeNull_WhenTweetOwnerDoseNotExist()
    {
        // Arrange
        var tweetId = "FCDCD6AD-3E4F-4790-BA09-AB6F45E56089";
        var tweetOwnerId = "EC9C9A8F-3FB6-455B-A807-72EB7C9243D5";

        var tweet = new Tweet
        {
            UserId = tweetId
        };

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(tweet);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(tweetOwnerId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.GetTweetByIdAsync(tweetId, tweetOwnerId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTweetByIdAsync_ShouldBeNull_WhenTweetCreatorDoseNotExist()
    {
        // Arrange
        var tweetId = "FCDCD6AD-3E4F-4790-BA09-AB6F45E56089";
        var tweetOwnerId = "EBE2E563-4EBD-445A-B7EA-FD95D084A367";

        var tweet = new Tweet
        {
            UserId = tweetId
        };

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(tweet);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(tweet.UserId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.GetTweetByIdAsync(tweetId, tweetOwnerId);

        // Assert
        result.Should().BeNull();
    }
}
