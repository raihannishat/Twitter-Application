using Application.Block.Shared.Interfaces;
using Application.IdentityManagement.User.Shared.Services;
using Microsoft.Extensions.Logging;

namespace Application.IdentityManagement.User.Queries.GetUsersByName;

public class GetUsersByNameQueryHandler : IRequestHandler<GetUsersByNameQuery, Result<List<UserViewModel>>>
{
    private readonly IUserService _userService;
    private readonly ILogger<GetUsersByNameQueryHandler> _logger;

    public GetUsersByNameQueryHandler(ILogger<GetUsersByNameQueryHandler> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<Result<List<UserViewModel>>> Handle(GetUsersByNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _userService.GetUsersByNameAsync(request.Name);
            
            _logger.LogInformation(QueryMessages<GetUsersByNameQuery>.QueryExecutedSuccessfully);
            
            return Result<List<UserViewModel>>.Success(users);

        }
        catch(Exception ex)
        {
            _logger.LogError(QueryMessages<GetUsersByNameQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<List<UserViewModel>>.Failure(QueryMessages<GetUsersByNameQuery>.FailedToExecuteQueryException);
        }
    }
}
