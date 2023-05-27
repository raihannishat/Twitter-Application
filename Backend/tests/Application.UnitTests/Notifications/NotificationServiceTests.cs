namespace Application.UnitTests.Notifications;

public class NotificationServiceTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<INotificationRepository> _notificationRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly NotificationService _sut;

    public NotificationServiceTests()
    {
        _currentUserServiceMock = new();
        _notificationRepositoryMock = new();
        _userRepositoryMock = new();

        _sut = new NotificationService(_currentUserServiceMock.Object,
            _notificationRepositoryMock.Object,
            _userRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateNotificationAsync_ShouldInsertNotification_WhenTweetOwnerAreNotEqualCurrentUser()
    {
        // Arrange
        var userId = "3C570E54-0368-4EAB-92E3-DF36124921E1";
        var currentUserId = "5A704013-5965-42BD-B8AD-420688C042B6";
        var action = "action type";

        var tweet = new Tweet
        {
            UserId = userId
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        // Act
        await _sut.CreateNotificationAsync(tweet, action);

        // Assert
        _notificationRepositoryMock.Verify(x =>
            x.InsertOneAsync(It.IsAny<Notification>()), Times.Once());
    }
}
