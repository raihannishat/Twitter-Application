namespace Application.IdentityManagement.Auth.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest<Result<Unit>>
{
    public string Token { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
