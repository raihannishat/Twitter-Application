using StackExchange.Redis;
using System.Security.Claims;

namespace Infrastructure.Identity;

public class JwtTokenService : IJwtTokenService
{
    public JwtTokenResult GenerateToken(User user, string secretKey)
    {
        var tokenHandlder = new JwtSecurityTokenHandler();

        var tokenKey = Encoding.ASCII.GetBytes(secretKey);

        var claims = new List<Claim>();

        claims.AddRange(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        });

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(10),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
            SecurityAlgorithms.HmacSha256Signature)
        };

        var tokens = new JwtSecurityToken(claims: claims);

        var token = tokenHandlder.CreateToken(tokenDescriptor);

        var refreshToken = new RefreshTokenDTO
        {
            Token = RandomString(32) + Guid.NewGuid(),
            //JwtId = token.Id,
            //UserId = user.Id,
            //CreationDate = DateTime.UtcNow,
            //ExpiryDate = DateTime.UtcNow.AddMonths(6)
        };

        return new JwtTokenResult
        {
            AccessToken = tokenHandlder.WriteToken(token),
            RefreshToken = refreshToken
        };
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token, string secretKey)
    {
        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = false,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validateToken);

            if (!IsJwtWithValidSecurityAlgorithm(validateToken))
            {
                return null!;
            }

            return principal;
        }
        catch
        {
            return null!;
        }
    }

    public static string RandomString(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Repeat(chars, length)
            .Select(x => x[random.Next(x.Length)]).ToArray());
    }
    public bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
            jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
    }
}
