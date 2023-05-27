namespace Application.IdentityManagement.Admin.Queries;

public class GetAllUsersQuery : IRequest<Result<List<UserProfileViewModel>>>
{
    public PaginationQueryRequest PageQuery { get; set; }

    public GetAllUsersQuery(PaginationQueryRequest paginationQuery) => PageQuery = paginationQuery;
}
