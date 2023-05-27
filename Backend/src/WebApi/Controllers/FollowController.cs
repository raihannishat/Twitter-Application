using Application.Follows.Commands.FollowUser;
using Application.Follows.Queries.GetFollowers;
using Application.Follows.Queries.GetFollowing;

namespace WebApi.Controllers;
[ApiController, Route("api/follow")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class FollowsController : BaseApiController
{
    private readonly ILogger<FollowsController> _logger;

    public FollowsController(ILogger<FollowsController> logger)
    {
        _logger = logger;
    }

    [HttpPost("user/{targetUserId}")]
    public async Task<IActionResult> Follow(string targetUserId)
    {
        return HandleResult(await Mediator.Send(new FollowUserCommand(targetUserId)));
    }


    [HttpGet("user/{userId}/followers")]
    public async Task<IActionResult> GetFollowers(string userId, [FromQuery(Name = "page")]int pageNumber)
    {
        var queryRequest = new GetFollowersQuery(userId, pageNumber);
        return HandleResult(await Mediator.Send(queryRequest));
    }

    [HttpGet("user/{userId}/following")]
    public async Task<IActionResult> GetFollowing(string userId, [FromQuery(Name = "page")] int pageNumber)
    {
        var queryRequest = new GetFollowingQuery(userId, pageNumber);
        return HandleResult(await Mediator.Send(queryRequest));
    }
}