using Application.Block.Shared.Interfaces;
using Application.Follows.Shared.Interfaces;
using Application.IdentityManagement.User.Shared.Services;
using Microsoft.Extensions.Logging;

namespace Application.IdentityManagement.User.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<List<UserViewModel>>>
{
    private readonly IUserService _userService;
    private readonly ILogger<GetUsersQueryHandler> _logger;

    public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<Result<List<UserViewModel>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userViewModels = await _userService.GetAllUsersAsync(request.PageQuery.PageNumber);

            return Result<List<UserViewModel>>.Success(userViewModels);
        }
        catch(Exception ex)
        {
            _logger.LogError(QueryMessages<GetUsersQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<List<UserViewModel>>.Failure(QueryMessages<GetUsersQuery>.FailedToExecuteQueryException);
        }
       
    }
}
