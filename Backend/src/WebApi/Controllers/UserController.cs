using Application.IdentityManagement.Shared.Interfaces;
using Application.IdentityManagement.User.Queries.GetUserEmail;
using Application.IdentityManagement.User.Queries.GetUsersByName;
using Bogus;
using Domain.Entities;

namespace WebApi.Controllers;

[ApiController, Route("api")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : BaseApiController
{
    private readonly ILogger<UserController> _logger;
    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers([FromQuery(Name = "page")] int pageNumber)
    {
        var pageQuery = new PaginationQueryRequest { PageNumber = pageNumber };
        return HandleResult(await Mediator.Send(new GetUsersQuery(pageQuery)));
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUser(CreateUserCommand createUser)
    {
        return HandleResult(await Mediator.Send(createUser));
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

    [AllowAnonymous]
    [HttpGet("users/email-exist/{email}")]
    public async Task<IActionResult> CheckEmailExist(string email)
    {
        return HandleResult(await Mediator.Send(new GetUserEmailQuery(email)));
    }

    [HttpPut("users/{id:length(24)}")]
    public async Task<IActionResult> Update(string id, UpdateUserCommand updateUser)
    {
        updateUser.Id = id;
        return HandleResult(await Mediator.Send(updateUser));
    }

    [HttpDelete("users/{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteUserCommand(id)));
    }
 
    [HttpGet("users/{name}")]
    public async Task<IActionResult> GetUserByNames(string name)
    {
        return HandleResult(await Mediator.Send(new GetUsersByNameQuery(name)));
    }
}
