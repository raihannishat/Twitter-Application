using Application.Tweets.Commands.CreateTweet;
using Application.Tweets.Commands.DeleteTweet;
using Application.Tweets.Commands.UpdateTweet;
using Application.Tweets.Queries.GetUserTimelineTweets;
//using Infrastructure.Persistence.RedisCaching;
using Application.Common.Interfaces;
using Application.Tweets.Queries.GetHomeTimelineTweets;
using Application.Tweets.Queries.GetTweetById;
using Application.Tweets.Queries.GetHashtagTweets;
using Application.Tweets.Queries.GetTrendingHashtag;
using Application.Likes.Commands;
using Application.Retweets.Commands;

namespace WebApi.Controllers;
[ApiController, Route("api")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TweetController : BaseApiController
{
    private readonly ILogger<TweetController> _logger;
    public TweetController(ILogger<TweetController> logger)
    {
        _logger = logger;
    }

    [HttpGet("tweets/{tweetId}")]
    public async Task<IActionResult> GetTweetById(string tweetId, [FromQuery]string tweetOwnerId)
    {
        return HandleResult(await Mediator.Send(new GetTweetByIdQuery(tweetId, tweetOwnerId)));
    }

    [HttpGet("tweets/hashtags")]
    public async Task<IActionResult> GetHashtags([FromQuery(Name = "page")] int pageNumber)
    {
        var queryRequest = new PaginationQueryRequest { PageNumber = pageNumber };
        return HandleResult(await Mediator.Send(new GetTrendingHashtagQuery(queryRequest)));
    }

    [HttpGet("tweets/hashtag-tweets")]
    public async Task<IActionResult> GetHashtagTweets([FromQuery]string keyword, [FromQuery(Name = "page")]int pageNumber)
    {
        var queryRequest = new PaginationQueryRequest { Keyword = '#' + keyword, PageNumber = pageNumber };

        return HandleResult(await Mediator.Send(new GetHashtagTweetsQuery(queryRequest)));
    }


    [HttpPost("tweets")]
    public async Task<IActionResult> Create(CreateTweetCommand createTweet)
    {
        return HandleResult(await Mediator.Send(createTweet));
    }


    [HttpPut("tweets/{id:length(24)}")]
    public async Task<IActionResult> Update(string id, UpdateTweetCommand updateTweet)
    {
        updateTweet.Id = id;
        return HandleResult(await Mediator.Send(updateTweet));
    }

    [HttpDelete("tweets/{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteTweetCommand(id)));
    }


    [HttpGet("tweets/home-timeline")]
    public async Task<IActionResult> GetHomeTimeline([FromQuery(Name = "page")]int pageNumber)
    {
        var pageQuery = new PaginationQueryRequest { PageNumber = pageNumber };

        return HandleResult(await Mediator.Send(new GetHomeTimelineQuery(pageQuery)));
    }

    [HttpGet("tweets/user-timeline")]
    public async Task<IActionResult> GetUserTimeline([FromQuery(Name = "user-id")]string userId, [FromQuery(Name = "page")] int pageNumber)
    {
        var pageQuery = new PaginationQueryRequest { PageNumber = pageNumber };
        
        var queryRequet = new GetUserTimelineQuery { UserId = userId, PageQuery = pageQuery };
       
        return HandleResult(await Mediator.Send(queryRequet));
    }

    [HttpPost("tweets/{tweet-id}/like")]
    public async Task<IActionResult> LikeTweet([FromRoute(Name = "tweet-id")]string tweetId)
    {
        var command = new LikeCommand { TweetId = tweetId };
        
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("tweets/{tweet-id}/retweet")]
    public async Task<IActionResult> Retweet([FromRoute(Name = "tweet-id")]string tweetId)
    {
        var command = new RetweetCommand { TweetId = tweetId };
        
        return HandleResult(await Mediator.Send(command));
    }
}
