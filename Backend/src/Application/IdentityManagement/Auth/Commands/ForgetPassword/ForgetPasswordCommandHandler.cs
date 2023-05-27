namespace Application.IdentityManagement.Auth.Commands.ForgetPassword;

public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Result<Unit>>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<ForgetPasswordCommandHandler> _logger;
    public ForgetPasswordCommandHandler(IIdentityService identityService, ILogger<ForgetPasswordCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _identityService.ForgetPassword(request.Email);

            _logger.LogInformation(CommandMessages<ForgetPasswordCommand>.CommandExecutedSuccessfully);

            return result;
        }
        catch(Exception ex)
        {
            _logger.LogError(CommandMessages<ForgetPasswordCommand>.FailedToExecuteCommandException, ex.Message);
            
            return Result<Unit>.Failure(CommandMessages<ForgetPasswordCommand>.FailedToExecuteCommandException);
        }
    }
}
