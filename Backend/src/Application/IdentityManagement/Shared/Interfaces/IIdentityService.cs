namespace Application.IdentityManagement.Shared.Interfaces;

public interface IIdentityService
{
    Task<Result<Unit>> Registration(SignUpCommand request);
    Task<Result<AuthResult>> Login(SignInCommand request);
    Task<Result<AuthResult>> RefreshToken(RefreshTokenCommand request);
    Task<Result<Unit>> VerifyAccount(string token);
    Task<Result<Unit>> ForgetPassword(string email);
    Task<Result<Unit>> ResetPassword(ResetPasswordDto request);
}
