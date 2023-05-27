namespace Application.IdentityManagement.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<Unit>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(IIdentityService identityService, IMapper mapper, ILogger<ResetPasswordCommandHandler> logger)
    {
        _identityService = identityService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var resetPasswordDto = _mapper.Map<ResetPasswordDto>(request);

            return await _identityService.ResetPassword(resetPasswordDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<ResetPasswordCommand>.FailedToExecuteCommandException, ex.Message);

            return Result<Unit>.Failure(CommandMessages<ResetPasswordCommand>.FailedToExecuteCommandException);
        }
    }
}
