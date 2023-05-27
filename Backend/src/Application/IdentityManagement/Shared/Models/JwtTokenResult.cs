namespace Application.IdentityManagement.Shared.Models;

public class JwtTokenResult
{
    public string AccessToken { get; set; } = string.Empty!;
    public RefreshTokenDTO RefreshToken { get; set; } = null!;
}
