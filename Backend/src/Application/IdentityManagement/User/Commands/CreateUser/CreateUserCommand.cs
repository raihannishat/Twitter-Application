namespace Application.IdentityManagement.User.Commands.CreateUser;

public class CreateUserCommand : IRequest<Result<Unit>>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
