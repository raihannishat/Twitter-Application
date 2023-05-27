namespace Application.Block.Commands;

public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, Result<BlockResponse>>
{
    private readonly IBlockUserService _blockUserService;
    private readonly ILogger<BlockUserCommandHandler> _logger;

    public BlockUserCommandHandler(IBlockUserService blockUserService, ILogger<BlockUserCommandHandler> logger)
    {
        _blockUserService = blockUserService;
        _logger = logger;
    }

    public async Task<Result<BlockResponse>> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var blockResult = await _blockUserService.BlockUserAsync(request.TargetUserId);

            if (blockResult != null)
            {
                var response = new BlockResponse
                {
                    IsBlocked = blockResult
                };

                _logger.LogInformation(CommandMessages<BlockUserCommand>.CommandExecutedSuccessfully);
                
                return Result<BlockResponse>.Success(response);
            }

            _logger.LogError(string.Format(CommandMessages<BlockUserCommand>.FailedToExecuteCommand));
            
            return Result<BlockResponse>.Failure(CommandMessages<BlockUserCommand>.FailedToExecuteCommand);
        }
        catch (Exception ex)
        {
            _logger.LogError(CommandMessages<BlockUserCommand>.FailedToExecuteCommandException, ex.Message);
            
            return Result<BlockResponse>.Failure(CommandMessages<BlockUserCommand>.FailedToExecuteCommandException);
        }
    }
}
