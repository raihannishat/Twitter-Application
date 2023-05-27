namespace Application.UnitTests.Tweets;

public class HashtagServiceTests
{
    private readonly Mock<ITweetRepository> _tweetRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IHashtagRepository> _hashtagRepositoryMock;
    private readonly Mock<IBlockUserService> _blockServiceMock;
    private readonly Mock<IRetweetService> _retweetServiceMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ISearchRepository> _searchRepositoryMock;
    private readonly Mock<ILikeService> _likeServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly HashtagService _sut;

    public HashtagServiceTests()
    {
        _tweetRepositoryMock = new();
        _currentUserServiceMock = new();
        _hashtagRepositoryMock = new();
        _blockServiceMock = new();
        _retweetServiceMock = new();
        _userRepositoryMock = new();
        _searchRepositoryMock = new();
        _likeServiceMock = new();
        _mapperMock = new();

        _sut = new HashtagService(_searchRepositoryMock.Object,
            _tweetRepositoryMock.Object,
            _userRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _hashtagRepositoryMock.Object,
            _mapperMock.Object,
            _blockServiceMock.Object,
            _retweetServiceMock.Object,
            _likeServiceMock.Object);
    }

    [Fact]
    public void ExtractHashTag_ShouldBeEmptyCollection_WhenDoseNotContainAnyHashTag()
    {
        // Arrange
        var content = "This is a sample content without any hashtags.";

        // Act
        var result = _sut.ExtractHashTag(content);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ExtractHashTag_ShouldBeCollection_WhenHashTagIsExist()
    {
        // Arrange
        var content = "This is a sample content with #hashtags and #multiple hashTags.";
        var expectedTags = new[] { "#hashtags", "#multiple" };

        // Act
        var result = _sut.ExtractHashTag(content);

        // Assert
        result.Should().Contain(expectedTags).And.HaveCount(expectedTags.Length);
    }

    [Fact]
    public async Task GetHashTagAsync_ShouldBeEmptyCollection_WhenTagDoseNotExist()
    {
        // Arrange
        var tagname = "example";

        _searchRepositoryMock.Setup(x => x.GetHashtagByRegex(tagname))
            .ReturnsAsync(() => null!);

        // Act
        var result = await _sut.GetHashTagAsync(tagname);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetHashTagAsync_ShouldReturnMappedHashtag_WhenTagIsExist()
    {
        // Arrange
        var tagname = "example";

        var search = new List<Search>
        {
            new Search { HashTag = "#.NET" },
            new Search { HashTag = "#Angular" },
            new Search { HashTag = "#AWS" },
        };

        _searchRepositoryMock.Setup(x => x.GetHashtagByRegex(tagname))
            .ReturnsAsync(search);

        //_mapperMock.Setup(mapper => mapper.Map<HashtagVM>(It.IsAny<string>()))
        //    .Returns((string tag) => new HashtagVM { HashTag = tag });

        // Act
        var result = await _sut.GetHashTagAsync(tagname);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task InsertHashtagInSearchTable_ShouldBeAddedSearch_WhenHashTagDoseNotExist()
    {
        // Arrange
        var existingTag = "existingTag";

        var hashtags = new List<string>
        {
            existingTag,
            "tag2",
            "tag3"
        };

        var searchExpression = new Search
        {
            HashTag = existingTag
        };

        _searchRepositoryMock.Setup(x =>
            x.FindOneByMatchAsync(It.Is<Expression<Func<Search, bool>>>(y =>
            y.Compile()(searchExpression))))
            .ReturnsAsync(() => null!);

        // Act
        await _sut.InsertHashtagInSearchTable(hashtags);

        // Assert
        _searchRepositoryMock.Verify(x =>
            x.InsertOneAsync(It.IsAny<Search>()), Times.Exactly(3));
    }

    [Fact]
    public async Task InsertHashtagInSearchTable_ShouldDoseNotAddedSearch_WhenHashTagIsExist()
    {
        // Arrange
        var existingTag = "existingTag";

        var hashtags = new List<string>
        {
            existingTag
        };

        var searchExpression = new Search
        {
            HashTag = existingTag
        };

        var searchResult = new Search
        {
            HashTag = existingTag
        };

        _searchRepositoryMock.Setup(x =>
            x.FindOneByMatchAsync(It.Is<Expression<Func<Search, bool>>>(y =>
            y.Compile()(searchExpression))))
            .ReturnsAsync(searchResult);

        // Act
        await _sut.InsertHashtagInSearchTable(hashtags);

        // Assert
        _searchRepositoryMock.Verify(x =>
            x.InsertOneAsync(It.IsAny<Search>()), Times.Never());
    }

    [Fact]
    public async Task ProcessTweetsHashtag_ShouldBeInsertHashtag_WhenTagsCountGreaterThanZero()
    {
        // Arrange
        var userId = "7FF22CF8-B32D-4AA9-AF18-9EF579BDA239";

        var tweet = new Tweet
        {
            Id = "23EB0A92-7A08-4323-AB24-2727AFFE90B7",
            Content = "This is my tweet and it's my #hashtag",
            CreatedAt = DateTime.UtcNow
        };

        _currentUserServiceMock.Setup(x => x.UserId)
            .Returns(userId);

        // Act
        await _sut.ProcessTweetsHashtag(tweet);

        // Assert
        _hashtagRepositoryMock.Verify(x =>
            x.InsertOneAsync(It.IsAny<Hashtag>()), Times.Once());
    }

    [Fact]
    public async Task GetHashtagTweetsAsync_ShouldBeNull_WhenHashTagNullOrEmpty()
    {
        // Arrange
        var keyword = string.Empty;
        var pageNumber = 1;

        // Act
        var result = await _sut.GetHashtagTweetsAsync(keyword, pageNumber);

        // Assert
        result.Should().BeNull();

        _hashtagRepositoryMock.Verify(x
            => x.GetHashtagTweetByDescendingTime(It.IsAny<Expression<Func<Hashtag, bool>>>(), It.IsAny<int>()), Times.Never());
    }

    [Fact]
    public async Task GetHashtagTweetsAsync_ShouldEmptyTweetCollection_WhenHashTagDoseNotExist()
    {
        // Arrange
        var keyword = "#Hashtag";
        var pageNumber = 1;

        var hashtagExpression = new Hashtag
        {
            TagName = keyword
        };

        var hashtagResult = new List<Hashtag>();

        _hashtagRepositoryMock.Setup(x =>
            x.GetHashtagTweetByDescendingTime(It.Is<Expression<Func<Hashtag, bool>>>(y =>
            y.Compile()(hashtagExpression)), pageNumber))
            .Returns(hashtagResult);

        // Act
        var result = await _sut.GetHashtagTweetsAsync(keyword, pageNumber);

        // Assert
        result.Should().BeEmpty();
    }
}
