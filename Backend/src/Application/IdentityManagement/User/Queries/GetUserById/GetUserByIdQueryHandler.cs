using Application.Block.Shared.Interfaces;
using Application.Follows.Shared.Interfaces;
using Application.IdentityManagement.User.Shared.Services;
using Microsoft.Extensions.Logging;

namespace Application.IdentityManagement.User.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserProfileDto>>
{
    private readonly IUserService _userService;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(ILogger<GetUserByIdQueryHandler> logger,IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<Result<UserProfileDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userService.GetUserByIdAsync(request.Id);

            if (result != null)
            {
                _logger.LogInformation(QueryMessages<GetUserByIdQuery>.QueryExecutedSuccessfully);
                
                return Result<UserProfileDto>.Success(result);
            }
            _logger.LogError(QueryMessages<GetUserByIdQuery>.FailedToExecuteQuery);

            return Result<UserProfileDto>.Failure(QueryMessages<GetUserByIdQuery>.FailedToExecuteQuery);
        }
        
        catch(Exception ex)
        {
            _logger.LogError(QueryMessages<GetUserByIdQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<UserProfileDto>.Failure(QueryMessages<GetUserByIdQuery>.FailedToExecuteQueryException);
        }
    }
}
