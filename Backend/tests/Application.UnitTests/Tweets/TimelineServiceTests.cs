using Application.Tweets.Shared.Models;

namespace Application.UnitTests.Tweets;

public class TimelineServiceTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IHomeTimelineRepository> _homeTimelineRepositoryMock;
    private readonly Mock<IUserTimelineRepository> _userTimelineRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILikeService> _likeServiceMock;
    private readonly Mock<IRetweetService> _retweetServiceMock;
    private readonly Mock<IBlockUserService> _blockServiceMock;
    private readonly Mock<IRetweetRepository> _retweetRepositoryMock;
    private readonly Mock<IBlockRepository> _blockRepositoryMock;
    private readonly Mock<ITweetRepository> _tweetRepositoryMock;
    private readonly Mock<ITweetViewModelFactory> _tweetViewModelFactoryMock;
    private readonly Mock<IFollowRepository> _followRepositoryMock;
    private readonly TimelineService _sut;

    public TimelineServiceTests()
    {
        _currentUserServiceMock = new();
        _homeTimelineRepositoryMock = new();
        _userTimelineRepositoryMock = new();
        _userRepositoryMock = new();
        _likeServiceMock = new();
        _retweetServiceMock = new();
        _blockServiceMock = new();
        _retweetRepositoryMock = new();
        _blockRepositoryMock = new();
        _tweetRepositoryMock = new();
        _tweetViewModelFactoryMock = new();
        _followRepositoryMock = new();

        _sut = new TimelineService(_currentUserServiceMock.Object,
            _homeTimelineRepositoryMock.Object,
            _userTimelineRepositoryMock.Object,
            _userRepositoryMock.Object,
            _retweetRepositoryMock.Object,
            _blockRepositoryMock.Object,
            _tweetRepositoryMock.Object,
            _tweetViewModelFactoryMock.Object,
            _followRepositoryMock.Object,
            _likeServiceMock.Object,
            _retweetServiceMock.Object,
            _blockServiceMock.Object);
    }

    [Fact]
    public async Task GetHomeTimelineAsync_ShouldBeEmpty_WhenTimelineDoseNotExistAnyContent()
    {
        // Arrange
        var pageNumber = 1;
        var currentUserId = "1CA802D9-42E2-4AD1-A3FC-572AB595045D";

        var homeTimelineExpression = new HomeTimeline
        {
            UserId = currentUserId
        };

        var homeTimelineResult = new List<HomeTimeline>();

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _homeTimelineRepositoryMock.Setup(x =>
            x.GetTweetByDescendingTime(It.Is<Expression<Func<HomeTimeline, bool>>>(y =>
            y.Compile()(homeTimelineExpression)), pageNumber))
            .Returns((IEnumerable<HomeTimeline>)homeTimelineResult);

        // Act
        var result = await _sut.GetHomeTimelineAsync(pageNumber);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUserTimelineAsync_ShouldEmptyTweetCollection_WhenUserDoseNotExist()
    {
        // Arrange
        var userId = "A0F91D4D-B7D5-4204-A29F-FB5022E41DD7";
        var pageNumber = 1;

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.GetUserTimelineAsync(userId, pageNumber);

        // Assert
        result.Should().BeEmpty();

        _userTimelineRepositoryMock.Verify(x
            => x.GetTweetByDescendingTime(It.IsAny<Expression<Func<UserTimeline, bool>>>(), pageNumber), Times.Never());
    }

    [Fact]
    public async Task GetUserTimelineAsync_ShouldEmptyTweetCollection_WhenUserTimelineDoseNotExist()
    {
        // Arrange
        var userId = "0C6E8E98-D076-4957-97A6-15943C339AF1";
        var pageNumber = 1;

        var userEntity = new User
        {
            Id = userId
        };

        var userTimelineExpression = new UserTimeline
        {
            UserId = userId
        };

        var userTimelines = new List<UserTimeline>();

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(userEntity);

        _userTimelineRepositoryMock.Setup(x =>
            x.GetTweetByDescendingTime(It.Is<Expression<Func<UserTimeline, bool>>>(y =>
            y.Compile()(userTimelineExpression)), pageNumber))
            .Returns(userTimelines);

        // Act
        var result = await _sut.GetUserTimelineAsync(userId, pageNumber);

        // Assert
        result.Should().BeEmpty();

        _currentUserServiceMock.Verify(x => x.UserId, Times.Never());
    }

    [Fact]
    public async Task GetUserTimelineAsync_ShouldBeNull_WhenTweetDoseNotExist()
    {
        // Arrange
        var userId = "B4E6949A-99C6-4720-B627-E2C4620A8E4C";
        var tweetId = "16EE8CB4-E98C-4829-BCDC-478128AFBC8A";
        var pageNumber = 1;

        var userEntity = new User
        {
            Id = userId
        };

        var userTimelineExpression = new UserTimeline
        {
            UserId = userId
        };

        var userTimelines = new List<UserTimeline>
        {
            new UserTimeline()
        };

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(userEntity);

        _userTimelineRepositoryMock.Setup(x =>
            x.GetTweetByDescendingTime(It.Is<Expression<Func<UserTimeline, bool>>>(y =>
            y.Compile()(userTimelineExpression)), pageNumber))
            .Returns(userTimelines);

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(userId);

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.GetUserTimelineAsync(userId, pageNumber);

        // Assert
        result.Should().BeEmpty();

        _tweetViewModelFactoryMock.Verify(x =>
            x.CreateTweetViewModelAsync(It.IsAny<Tweet>(), It.IsAny<User>()), Times.Never());
    }

    [Fact]
    public async Task GetUserTimelineAsync_ShouldAddTweetCollection_WhenTweetIsExist()
    {
        // Arrange
        var userId = "5BD5EEB5-3E15-45AF-A6A5-CCF87396A99F";
        var tweetId = "BAD2CDD4-67D9-49DD-BDE6-7D1E55252277";
        var pageNumber = 1;

        var userEntity = new User
        {
            Id = userId
        };

        var tweetEntity = new Tweet
        {
            Id = tweetId
        };

        var tweetViewModel = new TweetViewModel();

        var userTimelineExpression = new UserTimeline
        {
            UserId = userId
        };

        var userTimelines = new List<UserTimeline>
        {
            new UserTimeline() { TweetId = tweetId }
        };

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(userEntity);

        _userTimelineRepositoryMock.Setup(x =>
            x.GetTweetByDescendingTime(It.Is<Expression<Func<UserTimeline, bool>>>(y =>
            y.Compile()(userTimelineExpression)), pageNumber))
            .Returns(userTimelines);

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(userId);

        _tweetRepositoryMock.Setup(x => x.FindByIdAsync(tweetId))
            .ReturnsAsync(tweetEntity);

        _tweetViewModelFactoryMock.Setup(x => x.CreateTweetViewModelAsync(tweetEntity, userEntity))
            .ReturnsAsync(tweetViewModel);

        // Act
        var result = await _sut.GetUserTimelineAsync(userId, pageNumber);

        // Assert
        result.Equals(tweetViewModel);

        _tweetViewModelFactoryMock.Verify(x =>
            x.CreateTweetViewModelAsync(It.IsAny<Tweet>(), It.IsAny<User>()), Times.Once());
    }
}
