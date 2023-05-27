namespace Application.IdentityManagement.Shared.Interfaces;

public interface IJwtTokenService
{
    JwtTokenResult GenerateToken(Domain.Entities.User user, string secretKey);
    ClaimsPrincipal GetPrincipalFromToken(string token, string secretKey);
}
