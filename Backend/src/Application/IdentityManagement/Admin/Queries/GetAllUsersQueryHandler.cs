namespace Application.IdentityManagement.Admin.Queries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserProfileViewModel>>>
{
    private readonly IAdminService _adminBlockUserService;
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    public GetAllUsersQueryHandler(IAdminService adminBlockUserService,
        ILogger<GetAllUsersQueryHandler> logger)
    {
        _adminBlockUserService = adminBlockUserService;
        _logger = logger;
    }

    public async Task<Result<List<UserProfileViewModel>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _adminBlockUserService.GetAllUsersAsync(request.PageQuery.PageNumber);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetAllUsersQuery>.QueryExecutedSuccessfully);

                return Result<List<UserProfileViewModel>>.Success(response.ToList());
            }
            _logger.LogError(QueryMessages<GetAllUsersQuery>.FailedToExecuteQuery);

            return Result<List<UserProfileViewModel>>.Failure(QueryMessages<GetAllUsersQuery>.FailedToExecuteQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetAllUsersQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<List<UserProfileViewModel>>.Failure(QueryMessages<GetAllUsersQuery>.FailedToExecuteQueryException);
        }
    }
}
