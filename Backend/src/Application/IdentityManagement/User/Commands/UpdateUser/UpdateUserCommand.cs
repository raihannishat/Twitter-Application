namespace Application.IdentityManagement.User.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Result<Unit>>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

