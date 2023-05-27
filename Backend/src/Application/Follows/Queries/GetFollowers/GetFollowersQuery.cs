namespace Application.Follows.Queries.GetFollowers;

public class GetFollowersQuery : IRequest<Result<List<UserViewModel>>>
{
    public string UserId { get; set; }
    public int PageNumber { get; set; }

    public GetFollowersQuery(string userId, int pageNumber)
    {
        UserId = userId;
        PageNumber = pageNumber;
    }
}
