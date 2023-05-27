namespace Application.IdentityManagement.User.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Result<Unit>>
{
	public string Id { get; set; } = string.Empty;

	public DeleteUserCommand(string id) => Id = id;
}
