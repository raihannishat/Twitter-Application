namespace Application.UnitTests.Identity;

public class AdminServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AdminService _sut;

    public AdminServiceTests()
    {
        _userRepositoryMock = new();
        _mapperMock = new();

        _sut = new AdminService(_userRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task BlockUserAsync_ShouldBeNull_WhenUserDoseNotExist()
    {
        // Arrange
        var userId = "D10AA92D-DE2E-4EB3-B49A-4AE5F6685988";

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.BlockUserAsync(userId);

        // Assert
        result.Should().BeNull();

        _userRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(It.IsAny<User>()), Times.Never());
    }

    [Fact]
    public async Task BlockUserAsync_ShouldBeBlockOrUnBlockUser_WhenUserIsExist()
    {
        // Arrange
        var userId = "0CBF44AB-BD1A-4E2D-BA89-FC5037516040";

        var userResult = new User
        {
            Id = userId,
            IsBlockedByAdmin = false
        };

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(userResult);

        // Act
        var result = await _sut.BlockUserAsync(userId);

        // Assert
        result.Should().BeTrue();
        result.Should().Be(userResult.IsBlockedByAdmin);

        _userRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(userResult), Times.Once());
    }
}
