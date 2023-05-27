namespace Application.UnitTests.Photos;

public class PhotoServiceTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPhotoAccessor> _photoAccessorMock;
    private readonly Mock<IFormFile> _photoMock;
    private readonly PhotoService _sut;

    public PhotoServiceTests()
    {
        _currentUserServiceMock = new();
        _userRepositoryMock = new();
        _photoAccessorMock = new();
        _photoMock = new();

        _sut = new PhotoService(_currentUserServiceMock.Object,
            _userRepositoryMock.Object,
            _photoAccessorMock.Object);
    }

    [Fact]
    public async Task UploadCoverPhotoAsync_ShouldBeNull_WhenUserDoseNotExist()
    {
        // Arrange
        var userId = "FDD93BE4-27DC-44FD-8001-FEB90F50F2A9";
        var fileContent = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var stream = new MemoryStream(fileContent);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(() => null!);

        _photoMock.Setup(x => x.FileName)
            .Returns("example.jpg");

        _photoMock.Setup(x => x.Length)
            .Returns(1024);

        _photoMock.Setup(x => x.ContentType)
            .Returns("image/jpeg");

        _photoMock.Setup(f => f.OpenReadStream())
            .Returns(stream);

        // Act
        var result = await _sut.UploadCoverPhotoAsync(_photoMock.Object);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UploadProfilePhotoAsync_ShouldBeNull_WhenUserDoseNotExist()
    {
        // Arrange
        var userId = "6A7556DC-3A39-4393-9F8A-C10CD255925F";
        var fileContent = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var stream = new MemoryStream(fileContent);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(() => null!);

        _photoMock.Setup(x => x.FileName)
            .Returns("example.jpg");

        _photoMock.Setup(x => x.Length)
            .Returns(1024);

        _photoMock.Setup(x => x.ContentType)
            .Returns("image/jpeg");

        _photoMock.Setup(f => f.OpenReadStream())
            .Returns(stream);

        // Act
        var result = await _sut.UploadProfilePhotoAsync(_photoMock.Object);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UploadCoverPhotoAsync_ShouldBeNull_WhenCoverPhotoUploadFailed()
    {
        // Arrange
        var userId = "6A7556DC-3A39-4393-9F8A-C10CD255925F";
        var fileContent = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var stream = new MemoryStream(fileContent);

        var user = new User
        {
            Id = userId
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(userId);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _photoMock.Setup(x => x.FileName)
            .Returns("example.jpg");

        _photoMock.Setup(x => x.Length)
            .Returns(1024);

        _photoMock.Setup(x => x.ContentType)
            .Returns("image/jpeg");

        _photoMock.Setup(f => f.OpenReadStream())
            .Returns(stream);

        _photoAccessorMock.Setup(x => x.AddCoverPhotoAsync(_photoMock.Object))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.UploadCoverPhotoAsync(_photoMock.Object);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UploadProfilePhotoAsync_ShouldBeNull_WhenProfilePhotoUploadFailed()
    {
        // Arrange
        var userId = "C9903F5F-8D4A-4B6E-B9F5-9D9B1EAB111D";
        var fileContent = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var stream = new MemoryStream(fileContent);

        var user = new User
        {
            Id = userId
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(userId);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _photoMock.Setup(x => x.FileName)
            .Returns("example.jpg");

        _photoMock.Setup(x => x.Length)
            .Returns(1024);

        _photoMock.Setup(x => x.ContentType)
            .Returns("image/jpeg");

        _photoMock.Setup(f => f.OpenReadStream())
            .Returns(stream);

        _photoAccessorMock.Setup(x => x.AddProfilePhotoAsync(_photoMock.Object))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.UploadProfilePhotoAsync(_photoMock.Object);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UploadCoverPhotoAsync_ShouldBeCoverImage_WhenCoverPhotoUploadSuccess()
    {
        // Arrange
        var userId = "D7E4807B-80D9-4286-B94D-7B394EE02479";
        var fileContent = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var stream = new MemoryStream(fileContent);
        var publicId = "C9112F44-9676-43CE-A192-79E19734B9B3";
        var url = "https://example.cloud.com/cover_photo=?demo.jpg";

        var user = new User
        {
            Id = userId
        };

        var photoUploadResult = new PhotoUploadResult
        {
            PublicId = publicId,
            Url = url
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(userId);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _photoMock.Setup(x => x.FileName)
            .Returns("example.jpg");

        _photoMock.Setup(x => x.Length)
            .Returns(1024);

        _photoMock.Setup(x => x.ContentType)
            .Returns("image/jpeg");

        _photoMock.Setup(f => f.OpenReadStream())
            .Returns(stream);

        _photoAccessorMock.Setup(x => x.AddCoverPhotoAsync(_photoMock.Object))
            .ReturnsAsync(photoUploadResult);

        // Act
        var result = await _sut.UploadCoverPhotoAsync(_photoMock.Object);

        // Assert
        result.Should().BeSameAs(user.CoverImage);

        _userRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(user), Times.Once());
    }

    [Fact]
    public async Task UploadProfilePhotoAsync_ShouldBeCoverImage_WhenProfilePhotoUploadSuccess()
    {
        // Arrange
        var userId = "340C6C3C-4AA6-4BD4-A178-2B61683577FF";
        var fileContent = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var stream = new MemoryStream(fileContent);
        var publicId = "237AB7FB-05CF-41CC-8DBA-C4A59B054C1F";
        var url = "https://example.cloud.com/profile_photo=?demo.jpg";

        var user = new User
        {
            Id = userId
        };

        var photoUploadResult = new PhotoUploadResult
        {
            PublicId = publicId,
            Url = url
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(userId);

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(user);

        _photoMock.Setup(x => x.FileName)
            .Returns("example.jpg");

        _photoMock.Setup(x => x.Length)
            .Returns(1024);

        _photoMock.Setup(x => x.ContentType)
            .Returns("image/jpeg");

        _photoMock.Setup(f => f.OpenReadStream())
            .Returns(stream);

        _photoAccessorMock.Setup(x => x.AddProfilePhotoAsync(_photoMock.Object))
            .ReturnsAsync(photoUploadResult);

        // Act
        var result = await _sut.UploadProfilePhotoAsync(_photoMock.Object);

        // Assert
        result.Should().BeSameAs(user.Image);

        _userRepositoryMock.Verify(x =>
            x.ReplaceOneAsync(user), Times.Once());
    }
}
