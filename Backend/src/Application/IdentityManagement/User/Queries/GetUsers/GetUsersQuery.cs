namespace Application.IdentityManagement.User.Queries.GetUsers;

public class GetUsersQuery : IRequest<Result<List<UserViewModel>>>
{
    public PaginationQueryRequest PageQuery { get; set; }

    public GetUsersQuery(PaginationQueryRequest paginationQuery) => PageQuery = paginationQuery;
}
