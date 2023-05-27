namespace Application.UnitTests.Block;

public class BlockUserServiceTests
{
    private readonly BlockUserService _sut;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IBlockRepository> _blockRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public BlockUserServiceTests()
    {
        _currentUserServiceMock = new();
        _blockRepositoryMock = new();
        _userRepositoryMock = new();

        _sut = new BlockUserService(_currentUserServiceMock.Object,
            _userRepositoryMock.Object,
            _blockRepositoryMock.Object);
    }

    [Fact]
    public async Task BlockUserAsync_ShouldBlockAddedSuccessfully_WhenTargetUserExists()
    {
        // Arrange
        var userId = "01D52498-861A-43B4-8989-E602CF8C2703";
        var currentUserId = "58F1273C-9FB3-475E-84E2-D9D61704F3EE";
        var targetUser = new User { Id = userId };

        var blockEntity = new Blocks
        {
            BlockedById = currentUserId,
            BlockedId = targetUser.Id
        };

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(targetUser);

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _blockRepositoryMock.Setup(x =>
            x.FindOneByMatchAsync(It.Is<Expression<Func<Blocks, bool>>>(y =>
            y.Compile()(blockEntity))))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.BlockUserAsync(userId);

        //Assert
        result.Should().BeTrue();

        _blockRepositoryMock.Verify(x => x.InsertOneAsync(It.Is<Blocks>(block =>
            block.BlockedId == userId && block.BlockedById == currentUserId)), Times.Once());
    }

    [Fact]
    public async Task BlockUserAsync_ShouldReturnNull_WhenTargetUserNotFound()
    {
        // Arrange
        var userId = "CB5C321B-7932-4827-AEBE-51E58C20069E";

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.BlockUserAsync(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task BlockUserAsync_ShouldReturnNull_WhenSameUserIds()
    {
        // Arrange
        var userId = "EC7DAB97-C0A0-40A6-BD10-C4F9223C2295";

        _currentUserServiceMock.Setup(service => service.UserId)
            .Returns(userId);

        // Act
        var result = await _sut.BlockUserAsync(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task BlockUserAsync_ShouldRemoveBlockSuccessfully_WhenBlockAlreadyExists()
    {
        // Arrange
        var userId = "601954D5-9B13-4BD3-AC84-4236CED5BB17";
        var currentUserId = "71E4DEF5-24BC-4A22-BDE3-1779C9D0DEDD";
        var targetUser = new User { Id = userId };
        var existingBlockId = "95B7D908-20D9-4902-B0AC-9DD44EDDEDC0";

        var blockEntity = new Blocks
        {
            BlockedById = currentUserId,
            BlockedId = targetUser.Id
        };

        var blockResult = new Blocks
        {
            Id = existingBlockId
        };

        _userRepositoryMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(targetUser);

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(currentUserId);

        _blockRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Blocks, bool>>>(y =>
            y.Compile()(blockEntity))))
            .ReturnsAsync(blockResult);

        // Act
        var result = await _sut.BlockUserAsync(userId);

        result.Should().BeFalse();

        _blockRepositoryMock.Verify(x =>
            x.DeleteByIdAsync(blockResult.Id), Times.Once());
    }

    [Fact]
    public async Task IsBlockAsync_ShouldReturnFalse_WhenBlockUserDoseNotExist()
    {
        var blockedId = "EBE5CB00-65F7-4564-AAC3-82AA2A1BA3BF";
        var blockedById = "7CE1D1A9-C64C-4DA0-9308-9060E93263A9";

        var blockEntity = new Blocks
        {
            BlockedById = blockedId,
            BlockedId = blockedById
        };

        _blockRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Blocks, bool>>>(y =>
            y.Compile()(blockEntity))))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.IsBlockAsync(blockedId, blockedById);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsBlockAsync_ShouldReturnTrue_WhenBlockUserExist()
    {
        var blockId = "A9D899A6-AD97-41A4-AE36-46DBFBE341CC";
        var blockedId = "EBE5CB00-65F7-4564-AAC3-82AA2A1BA3BF";
        var blockedById = "7CE1D1A9-C64C-4DA0-9308-9060E93263A9";

        var blockEntity = new Blocks
        {
            BlockedById = blockedById,
            BlockedId = blockedId
        };

        var blockResult = new Blocks
        {
            Id = blockId
        };

        _blockRepositoryMock.Setup(x => x.FindOneByMatchAsync(It.Is<Expression<Func<Blocks, bool>>>(y =>
            y.Compile()(blockEntity))))
            .ReturnsAsync(blockResult);

        // Act
        var result = await _sut.IsBlockAsync(blockedId, blockedById);

        // Assert
        result.Should().BeTrue();
    }

}

