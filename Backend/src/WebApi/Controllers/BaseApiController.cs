namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices
        .GetService<IMediator>()!;

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null)
        {
            return NotFound();
        }

        if (result.IsSuccess && result.Value != null)
        {
            return Ok(result.Value);
        }

        if (result.IsSuccess && result.Value == null)
        {
            return NotFound();
        }

        return BadRequest(result.Error);
    }

    protected ActionResult HandlePageResult<T>(Result<PagedResponse<T>> result)
    {
        if (result == null) return NotFound();

        if (result.IsSuccess && result.Value != null)
        {
            Response.AddPaginationHeader(result.Value.CurrentPage, result.Value.PageSize,
                result.Value.TotalCount, result.Value.TotalPage);

            return Ok(result.Value);
        }

        if (result.IsSuccess && result.Value == null)
        {
            return NotFound();
        }

        return BadRequest(result.Error);
    }
}
