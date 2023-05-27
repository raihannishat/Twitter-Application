namespace Application.IdentityManagement.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResult>>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;
    public RefreshTokenCommandHandler(IIdentityService identityService, ILogger<RefreshTokenCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<Result<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _identityService.RefreshToken(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<RefreshTokenCommand>.FailedToExecuteCommandException, ex.Message);

            return Result<AuthResult>.Failure(CommandMessages<RefreshTokenCommand>.FailedToExecuteCommandException);
        }
    }
}
