namespace Application.IdentityManagement.Admin.Commands;

public class AdminBlockUserCommandHandler : IRequestHandler<AdminBlockUserCommand, Result<AdminBlockResponse>>
{
    private readonly IAdminService _adminService;
    private readonly ILogger<AdminBlockUserCommandHandler> _logger;

    public AdminBlockUserCommandHandler(IAdminService adminBlockUserService,
        ILogger<AdminBlockUserCommandHandler> logger)
    {
        _adminService = adminBlockUserService;
        _logger = logger;
    }

    public async Task<Result<AdminBlockResponse>> Handle(AdminBlockUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var blockResult = await _adminService.BlockUserAsync(request.Id);

            if (blockResult != null)
            {
                var blockResponse = new AdminBlockResponse
                {
                    IsBlocked = blockResult
                };
                _logger.LogInformation(CommandMessages<AdminBlockUserCommand>.CommandExecutedSuccessfully);
            
                return Result<AdminBlockResponse>.Success(blockResponse);
            }
            _logger.LogError(CommandMessages<AdminBlockUserCommand>.FailedToExecuteCommand);    
            
            return Result<AdminBlockResponse>.Failure(CommandMessages<AdminBlockUserCommand>.FailedToExecuteCommand);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<AdminBlockUserCommand>.FailedToExecuteCommandException, ex.Message);

            return Result<AdminBlockResponse>.Failure(CommandMessages<AdminBlockUserCommand>.FailedToExecuteCommandException);
        }
    }
}
