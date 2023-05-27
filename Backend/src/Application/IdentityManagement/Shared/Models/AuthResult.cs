namespace Application.IdentityManagement.Shared.Models;

public class AuthResult
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public UserProfileViewModel? User { get; set; }
}
