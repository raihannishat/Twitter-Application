namespace WebApi.Controllers;

[ApiController, Route("api")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CommentController : BaseApiController
{
    [HttpGet("comments/tweet/{tweetId}")]
    public async Task<IActionResult> GetAllComments(string tweetId, [FromQuery(Name = "page")]int pageNumber)
    {
        var queryRequest = new PaginationQueryRequest { PageNumber = pageNumber };
        return HandleResult(await Mediator.Send(new GetCommentQuery(queryRequest, tweetId)));
    }

    [HttpPost("comments")]
    public async Task<IActionResult> Create(CreateCommentCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpDelete("comments/{commentId}/tweet/{tweetId}")]
    public async Task<IActionResult> Delete(string commentId, string tweetId)
    {
        var command = new DeleteCommentCommand(tweetId, commentId);
        return HandleResult(await Mediator.Send(command));
    }
}
