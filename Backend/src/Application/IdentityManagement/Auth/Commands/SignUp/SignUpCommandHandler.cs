namespace Application.IdentityManagement.Auth.Commands.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Result<Unit>>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<SignUpCommandHandler> _logger;

    public SignUpCommandHandler(IIdentityService identityService, ILogger<SignUpCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _identityService.Registration(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<SignUpCommand>.FailedToExecuteCommandException, ex.Message);

            return Result<Unit>.Failure(CommandMessages<SignUpCommand>.FailedToExecuteCommandException);
        }
    }
}
