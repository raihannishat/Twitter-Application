namespace Application.IdentityManagement.Auth.Commands.SignIn;

public class SignInCommand : IRequest<Result<AuthResult>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
