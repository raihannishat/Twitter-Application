namespace Application.Block.Queries;

public class GetBlockUsersQueryHandler : IRequestHandler<GetBlockUsersQuery, Result<List<UserViewModel>>>
{
    private readonly IBlockUserService _blockUserQueryService;
    private readonly ILogger<GetBlockUsersQueryHandler> _logger;

    public GetBlockUsersQueryHandler(IBlockUserService blockUserQueryService,
        ILogger<GetBlockUsersQueryHandler> logger)
    {
        _blockUserQueryService = blockUserQueryService;
        _logger = logger;
    }

    public async Task<Result<List<UserViewModel>>> Handle(GetBlockUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _blockUserQueryService.GetBlockedUsersAsync(request.PageQuery.PageNumber);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetBlockUsersQuery>.QueryExecutedSuccessfully);

                return Result<List<UserViewModel>>.Success(response.ToList());
            }
            else
            {
                _logger.LogError(QueryMessages<GetBlockUsersQuery>.FailedToExecuteQuery);

                return Result<List<UserViewModel>>.Failure(QueryMessages<GetBlockUsersQuery>.FailedToExecuteQuery);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetBlockUsersQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<List<UserViewModel>>.Failure(QueryMessages<GetBlockUsersQuery>.FailedToExecuteQueryException);
        }
    }
}
