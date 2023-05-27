using Application.Common.Interfaces;

namespace WebApi.Controllers;

[ApiController, Route("api/auth")]
public class AuthController : BaseApiController
{
    public AuthController() { }

    [HttpPost, Route("sign-up")]
    public async Task<IActionResult> Registration(SignUpCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost, Route("sign-in")]
    public async Task<IActionResult> Login(SignInCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost, Route("verify-account")]
    public async Task<IActionResult> VerifyAccount([FromQuery]string token)
    {
        return HandleResult(await Mediator.Send(new VerifyAccountCommand(token)));
    }

    [HttpPost, Route("forget-password")]
    public async Task<IActionResult> ForgetPassword(string email)
    {
        return HandleResult(await Mediator.Send(new ForgetPasswordCommand(email)));
    }

    [HttpPost, Route("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost, Route("refreshToken")]
    public async Task<IActionResult> Refresh(RefreshTokenCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }
}
