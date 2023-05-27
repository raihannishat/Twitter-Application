using Microsoft.Extensions.Logging;

namespace Application.IdentityManagement.User.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<Unit>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public DeleteUserCommandHandler(IUserRepository userRepository,
        IMapper mapper,
        ILogger<DeleteUserCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // not using anymore this handler. Created for this architecture testing purpose.

        var entity = await _userRepository.FindByIdAsync(request.Id);

        if (entity == null)
        {
            return null!;
        }
       
        await _userRepository.DeleteOneAsync(x => x.Id == request.Id);

        return Result<Unit>.Success(Unit.Value);
    }
}
