namespace Application.IdentityManagement.Auth.Commands.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand, Result<AuthResult>>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<SignInCommandHandler> _logger;

    public SignInCommandHandler(IIdentityService identityService, ILogger<SignInCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<Result<AuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _identityService.Login(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<SignInCommand>.FailedToExecuteCommandException, ex.Message);

            return Result<AuthResult>.Failure(CommandMessages<SignInCommand>.FailedToExecuteCommandException);
        }
    }
}
