namespace Application.Follows.Queries.GetFollowing;

public class GetFollowingQueryHandler : IRequestHandler<GetFollowingQuery, Result<List<UserViewModel>>>
{
    private readonly IFollowUserService _followUserService;
    private readonly ILogger<FollowUserCommandHandler> _logger;

    public GetFollowingQueryHandler(IFollowUserService followUserService,
        ILogger<FollowUserCommandHandler> logger)
    {
        _followUserService = followUserService;
        _logger = logger;
    }

    public async Task<Result<List<UserViewModel>>> Handle(GetFollowingQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _followUserService.GetFollowingAsync(request.UserId!, request.PageNumber);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetFollowingQuery>.QueryExecutedSuccessfully);
                return Result<List<UserViewModel>>.Success(response.ToList());
            }

            _logger.LogError(QueryMessages<GetFollowingQuery>.FailedToExecuteQuery);
            return Result<List<UserViewModel>>.Failure(QueryMessages<GetFollowingQuery>.FailedToExecuteQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetFollowingQuery>.FailedToExecuteQueryException, ex.Message);
            return Result<List<UserViewModel>>.Failure(QueryMessages<GetFollowingQuery>.FailedToExecuteQueryException);
        }
    }
}
