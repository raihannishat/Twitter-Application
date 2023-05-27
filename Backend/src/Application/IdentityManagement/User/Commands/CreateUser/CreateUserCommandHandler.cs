using Microsoft.Extensions.Logging;

namespace Application.IdentityManagement.User.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Unit>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(IUserRepository userRepository,
        IMapper mapper,
        ILogger<CreateUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // not using anymore this handler. Created for this architecture testing purpose.

        var entity = _mapper.Map<Domain.Entities.User>(request);

        await _userRepository.InsertOneAsync(entity);

        return Result<Unit>.Success(Unit.Value);
    }
}
