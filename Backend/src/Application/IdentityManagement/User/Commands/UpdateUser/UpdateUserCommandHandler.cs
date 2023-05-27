using Microsoft.Extensions.Logging;

namespace Application.IdentityManagement.User.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<Unit>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public UpdateUserCommandHandler(IUserRepository userRepository,
        IMapper mapper,
        ILogger<UpdateUserCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Unit>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // not using anymore this handler. Created for this architecture testing purpose.

        var entity = await _userRepository.FindByIdAsync(request.Id);

        if (entity == null)
        {
            return null!;
        }

        await _userRepository.ReplaceOneAsync(_mapper.Map<Domain.Entities.User>(request));

        return Result<Unit>.Success(Unit.Value);
    }
}
