namespace Application.Follows.Commands.FollowUser;

public class FollowUserCommand : IRequest<Result<FollowResponse>>
{
    public string TargetUserId { get; set; } = null!;

    public FollowUserCommand(string id) => TargetUserId = id;
}
