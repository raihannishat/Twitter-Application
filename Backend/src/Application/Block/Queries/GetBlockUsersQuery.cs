namespace Application.Block.Queries;

public class GetBlockUsersQuery : IRequest<Result<List<UserViewModel>>>
{
    public PaginationQueryRequest PageQuery { get; set; }
    public GetBlockUsersQuery(PaginationQueryRequest paginationQuery) => PageQuery = paginationQuery;
}
