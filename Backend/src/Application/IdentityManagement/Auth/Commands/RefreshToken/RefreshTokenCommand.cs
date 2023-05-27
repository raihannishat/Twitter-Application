namespace Application.IdentityManagement.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<Result<AuthResult>>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
