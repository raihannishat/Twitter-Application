namespace Application.IdentityManagement.User.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<Result<UserProfileDto>>
{
    public string Id { get; set; } = string.Empty;
    public GetUserByIdQuery(string id) => Id = id;
}
