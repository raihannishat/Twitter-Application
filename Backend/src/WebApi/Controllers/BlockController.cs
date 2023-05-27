using Application.Block.Commands;
using Application.Block.Queries;

namespace WebApi.Controllers;


[ApiController, Route("api/blocks")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BlockController : BaseApiController
{
    [HttpPost("users/{userId}/block")]
    public async Task<IActionResult> BlockUser(string userId)
    {
        return HandleResult(await Mediator.Send(new BlockUserCommand(userId)));
    }

    [HttpGet("users/blocked-users")]
    public async Task<IActionResult> GetBlockedUsers([FromQuery(Name = "page")]int pageNumber)
    {
        var queryRequest = new PaginationQueryRequest { PageNumber = pageNumber };
        return HandleResult(await Mediator.Send(new GetBlockUsersQuery(queryRequest)));
    }
}