namespace Application.IdentityManagement.Auth.Commands.ForgetPassword;

public class ForgetPasswordCommand : IRequest<Result<Unit>>
{
    public string Email { get; set; }

    public ForgetPasswordCommand(string email) => Email = email;
}
