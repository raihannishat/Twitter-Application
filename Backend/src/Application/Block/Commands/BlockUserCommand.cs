namespace Application.Block.Commands;

public class BlockUserCommand : IRequest<Result<BlockResponse>>
{
    public string TargetUserId { get; set; }
    public BlockUserCommand(string id) => TargetUserId = id;
}
