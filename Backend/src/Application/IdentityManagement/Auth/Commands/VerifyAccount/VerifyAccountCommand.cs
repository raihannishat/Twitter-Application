namespace Application.IdentityManagement.Auth.Commands.VerifyAccount;

public class VerifyAccountCommand : IRequest<Result<Unit>>
{
    public string Token { get; set; }

    public VerifyAccountCommand(string token)
    {
        Token = token;
    }
}
