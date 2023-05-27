namespace Application.IdentityManagement.Auth.Commands.VerifyAccount;

public class VerifyAccountCommandHandler : IRequestHandler<VerifyAccountCommand, Result<Unit>>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<VerifyAccountCommandHandler> _logger;

    public VerifyAccountCommandHandler(IIdentityService identityService, ILogger<VerifyAccountCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _identityService.VerifyAccount(request.Token);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<VerifyAccountCommand>.FailedToExecuteCommandException, ex.Message);

            return Result<Unit>.Failure(CommandMessages<VerifyAccountCommand>.FailedToExecuteCommandException);
        }
    }
}
