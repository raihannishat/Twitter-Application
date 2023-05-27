namespace WebApi.Controllers;

[ApiController, Route("api/admin")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class AdminController : BaseApiController
{
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger)
    {
        _logger = logger;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers([FromQuery(Name = "page")] int pageNumber)
    {
        var query = new PaginationQueryRequest { PageNumber = pageNumber};
        return HandleResult(await Mediator.Send(new GetAllUsersQuery(query)));
    }

    [HttpGet("users/{id:length(24)}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        return HandleResult(await Mediator.Send(new GetUserByIdQuery(id)));
    }

    [HttpGet("users/email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        return HandleResult(await Mediator.Send(new GetUserByEmailQuery(email)));
    }

    [HttpPut("users/{id:length(24)}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserCommand updateUser)
    {
        updateUser.Id = id;
        return HandleResult(await Mediator.Send(updateUser));
    }

    [HttpPost("users/{id}/block")]
    public async Task<IActionResult> BlockUser(string id)
    {
        return HandleResult(await Mediator.Send(new AdminBlockUserCommand(id)));
    }

    [HttpDelete("users/{id:length(24)}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteUserCommand(id)));
    }
}
