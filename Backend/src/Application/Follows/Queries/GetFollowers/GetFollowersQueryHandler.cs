namespace Application.Follows.Queries.GetFollowers;

public class GetFollowersQueryHandler : IRequestHandler<GetFollowersQuery, Result<List<UserViewModel>>>
{
    private readonly IFollowUserService _followUserService;
    private readonly ILogger<GetFollowersQueryHandler> _logger;

    public GetFollowersQueryHandler(IFollowUserService followUserService,
        ILogger<GetFollowersQueryHandler> logger)
    {
        _followUserService = followUserService;
        _logger = logger;
    }

    public async Task<Result<List<UserViewModel>>> Handle(GetFollowersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _followUserService.GetFollowersAsync(request.UserId!, request.PageNumber);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetFollowersQuery>.QueryExecutedSuccessfully);
                
                return Result<List<UserViewModel>>.Success(response.ToList());
            }
            _logger.LogError(QueryMessages<GetFollowersQuery>.FailedToExecuteQuery);
            
            return Result<List<UserViewModel>>.Failure(QueryMessages<GetFollowersQuery>.FailedToExecuteQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetFollowersQuery>.FailedToExecuteQueryException, ex.Message);
            
            return Result<List<UserViewModel>>.Failure(QueryMessages<GetFollowersQuery>.FailedToExecuteQueryException);
        }
    }
}
