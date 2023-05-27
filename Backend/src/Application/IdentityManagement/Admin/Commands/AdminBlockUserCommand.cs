namespace Application.IdentityManagement.Admin.Commands;

public class AdminBlockUserCommand : IRequest<Result<AdminBlockResponse>>
{
    public string Id { get; set; } = string.Empty;

    public AdminBlockUserCommand(string id) => Id = id;
}
