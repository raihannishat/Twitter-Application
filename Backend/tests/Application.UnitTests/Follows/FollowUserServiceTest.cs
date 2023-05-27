namespace Application.UnitTests.Follows;

public class FollowUserServiceTest
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IFollowRepository> _followRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ITimelineService> _timelineServiceMock;
    private readonly Mock<IBlockRepository> _blockRepositoryMock;
    private readonly FollowUserService _sut;

    public FollowUserServiceTest()
    {
        _currentUserServiceMock = new();
        _userRepositoryMock = new();
        _followRepositoryMock = new();
        _timelineServiceMock = new();
        _blockRepositoryMock = new();

        _sut = new FollowUserService(
            _followRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _userRepositoryMock.Object,
            _blockRepositoryMock.Object,
            _timelineServiceMock.Object);
    }

    [Fact]
    public async Task FollowUserAsync_ShouldBeNull_WhenCurrentUserAndTargetUserAreSame()
    {
        // Arrange
        var targetUserId = "9AC64A44-32FD-4C3D-9BB4-26229B679725";
        var currentUserId = targetUserId; // Set currentUserId to match targetUserId

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        // Act
        var result = await _sut.FollowUserAsync(targetUserId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task FollowUserAsync_ShouldBeNull_WhenTargetUserDoseNotExist()
    {
        // Arrange
        var targetUserId = "9AC64A44-32FD-4C3D-9BB4-26229B679725";
        var currentUserId = "2304073B-BE21-46B7-8A28-B5D7253A10C7";

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(targetUserId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.FollowUserAsync(targetUserId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task FollowUserAsync_ShouldUnfollowUser_WhenAlreadyFollowing()
    {
        // Arrange
        var targetUserId = "77F80815-9AC2-4CC2-B679-E0BDD36FED21";
        var currentUserId = "8F4EEF53-9DBE-4F74-BBE1-54B62DF7FE84";
        var followId = "03301B95-C1C3-492B-A2B7-4ACA8C1FE260";

        var currentUser = new User
        {
            Followers = 10
        };

        var targetUser = new User();

        var followEntity = new Follow
        {
            Id = followId,
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(currentUserId))
            .ReturnsAsync(currentUser);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(targetUserId))
            .ReturnsAsync(targetUser);

        // Set followExpression to indicate that the current user is already following the target user
        _followRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.IsAny<Expression<Func<Follow, bool>>>()))
            .ReturnsAsync(followEntity);

        // Act
        var result = await _sut.FollowUserAsync(targetUserId);

        // Assert
        result.Should().NotBeNull();
        result.Value.IsFollowing.Should().BeFalse();
        result.Value.Followers.Should().Be(targetUser.Followers);

        _followRepositoryMock.Verify(x =>
            x.FindOneByMatchAsync(It.IsAny<Expression<Func<Follow, bool>>>()), Times.Once());

        _timelineServiceMock.Verify(x =>
            x.DeleteTweetFromUserHomeTimeline(currentUserId, targetUserId), Times.Once());
    }

    [Fact]
    public async Task FollowUserAsync_ShouldFollowUser_WhenUserNotFollowing()
    {
        // Arrange
        var targetUserId = "C3A287E3-1780-4D4C-9A12-503315CD96D7";
        var currentUserId = "F726B322-6FB8-48DF-AA2B-ED67266446BD";

        var currentUser = new User
        {
            Followers = 10
        };

        var targetUser = new User();

        var followExpression = new Follow
        {
            FollowerId = currentUserId,
            FollowedId = targetUserId
        };

        var followEntity = new Follow
        {
            FollowerId = currentUserId,
            FollowedId = targetUserId
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(currentUserId))
            .ReturnsAsync(currentUser);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(targetUserId))
            .ReturnsAsync(targetUser);

        // Set followExpression to indicate that the current user is already following the target user
        _followRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Follow, bool>>>(y =>
            y.Compile()(followExpression))))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.FollowUserAsync(targetUserId);

        // Assert
        result.Value.IsFollowing.Should().BeTrue();
        result.Value.Followers.Should().Be(1);

        _timelineServiceMock.Verify(x =>
            x.AddUserTweetsToTimeline(currentUser, targetUser), Times.Once());
    }

    [Fact]
    public async Task GetFollowersAsync_ShouldBeNull_WhenUserNotExist()
    {
        // Arrange
        var userId = "4FB1F42D-CABA-47F7-A5C8-463E1DC626E1";
        var pageNumber = 1;

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.GetFollowersAsync(userId, pageNumber);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetFollowersAsync_ShouldBeNull_WhenFollowerDoseNotExist()
    {
        // Arrange
        var userId = "8C7DEB66-A85A-4F45-A74C-C4C74B84AABC";
        var pageNumber = 1;

        var userEntity = new User
        {
            Id = userId
        };

        var followExpression = new Follow
        {
            FollowedId = userId
        };

        var followerResult = new List<Follow>();

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(userEntity);

        _followRepositoryMock.Setup(x => x.FindByMatchWithPagination(It.Is<Expression<Func<Follow, bool>>>(y =>
            y.Compile()(followExpression)), pageNumber))
            .Returns(followerResult);

        // Act
        var result = await _sut.GetFollowersAsync(userId, pageNumber);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetFollowingAsync_ShouldBeNull_WhenUserNotExist()
    {
        // Arrange
        var userId = "24756F63-1488-486F-8455-37F96314DA93";
        var pageNumber = 1;

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.GetFollowingAsync(userId, pageNumber);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetFollowingAsync_ShouldBeNull_WhenFollowerDoseNotExist()
    {
        // Arrange
        var userId = "58841E0B-5429-44EA-B622-92E7006903AF";
        var pageNumber = 1;

        var userEntity = new User
        {
            Id = userId
        };

        var followExpression = new Follow
        {
            FollowedId = userId
        };

        var followerResult = new List<Follow>();

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(userEntity);

        _followRepositoryMock.Setup(x => x.FindByMatchWithPagination(It.Is<Expression<Func<Follow, bool>>>(y =>
            y.Compile()(followExpression)), pageNumber))
            .Returns(followerResult);

        // Act
        var result = await _sut.GetFollowingAsync(userId, pageNumber);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task IsFollowing_ShouldBeTrue_WhenFollowerUserAndFollowedUserAreExist()
    {
        // Arrange
        var followerId = "7491BD87-6B96-438F-91A8-09F6A2A6C4BA";
        var followedId = "C5006C07-8CD7-49FB-BBD4-E895A2DEC29C";

        var followExpression = new Follow
        {
            FollowedId = followedId,
            FollowerId = followerId
        };

        var followResult = new Follow();

        _followRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Follow, bool>>>(y =>
            y.Compile()(followExpression))))
            .ReturnsAsync(followResult);

        // Act
        var result = await _sut.IsFollowing(followerId, followedId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsFollowing_ShouldBeFalse_WhenFollowerUserAndFollowedUserAreDoseNotExist()
    {
        // Arrange
        var followerId = "48981677-9C20-4A97-99DA-CE09BFE936B3";
        var followedId = "C9C5ED4B-6578-4523-BCBC-D80DCE830EB8";

        var followExpression = new Follow
        {
            FollowedId = followerId,
            FollowerId = followedId
        };

        _followRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Follow, bool>>>(y =>
            y.Compile()(followExpression))))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.IsFollowing(followerId, followedId);

        // Assert
        result.Should().BeFalse();
    }

    //[Fact]
    //public async Task FollowUserAsync_ShouldReturnIsFollowingAndFollowers()
    //{
    //    // Arrange
    //    var targetUserId = "targetUserId";
    //    var currentUserId = "currentUserId";
    //    var currentUser = new User();
    //    var targetUser = new User();
    //    currentUser.Followers = 10;

    //    var currentUserServiceMock = new Mock<ICurrentUserService>();
    //    currentUserServiceMock.Setup(c => c.UserId).Returns(currentUserId);

    //    var userRepositoryMock = new Mock<IUserRepository>();
    //    userRepositoryMock.Setup(r => r.FindByIdAsync(currentUserId)).ReturnsAsync(currentUser);
    //    userRepositoryMock.Setup(r => r.FindByIdAsync(targetUserId)).ReturnsAsync(targetUser);

    //    var followRepositoryMock = new Mock<IFollowRepository>();
    //    followRepositoryMock.Setup(r => r.FindOneByMatchAsync(It.IsAny<Expression<Func<Follow, bool>>>()))
    //        .ReturnsAsync((Follow)null!);



    //    // Act
    //    var result = await _sut.FollowUserAsync(targetUserId);

    //    // Assert
    //    result.Should().NotBeNull();

    //    result.Should().Be((true, targetUser.Followers));

    //    userRepositoryMock.Verify(r => r.FindByIdAsync(currentUserId), Times.Once);
    //    userRepositoryMock.Verify(r => r.FindByIdAsync(targetUserId), Times.Once);
    //    followRepositoryMock.Verify(r => r.FindOneByMatchAsync(It.IsAny<Expression<Func<Follow, bool>>>()),
    //        Times.Once);
    //    _timelineServiceMock.Verify(s => s.DeleteTweetFromUserHomeTimeline(currentUserId, targetUserId),
    //        Times.Never);

    //    _timelineServiceMock.Verify(s => s.AddUserTweetsToTimeline(currentUser, targetUser), Times.Once);
    //}
}
