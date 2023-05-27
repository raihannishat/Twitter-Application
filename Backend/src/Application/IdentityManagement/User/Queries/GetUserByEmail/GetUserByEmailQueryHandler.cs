using Microsoft.Extensions.Logging;

namespace Application.IdentityManagement.User.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, Result<UserProfileViewModel>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserByEmailQueryHandler> _logger;
    public GetUserByEmailQueryHandler(IUserRepository userRepository, IMapper mapper,
        ILogger<GetUserByEmailQueryHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<UserProfileViewModel>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _userRepository.FindOneAsync(x => x.Email == request.Email);

            if (entity == null)
            {
                return null!;
            }

            var userViewModel = _mapper.Map<UserProfileViewModel>(entity);

            return Result<UserProfileViewModel>.Success(userViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetUserByEmailQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<UserProfileViewModel>.Failure(QueryMessages<GetUserByEmailQuery>.FailedToExecuteQueryException);
        }
    }
}
