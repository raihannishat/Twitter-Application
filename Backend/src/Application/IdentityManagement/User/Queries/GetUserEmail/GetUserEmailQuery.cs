namespace Application.IdentityManagement.User.Queries.GetUserEmail;

public class GetUserEmailQuery : IRequest<Result<bool>>
{
    public string Email { get; set; } = string.Empty;

    public GetUserEmailQuery(string email)
    {
        Email = email;
    }
}
