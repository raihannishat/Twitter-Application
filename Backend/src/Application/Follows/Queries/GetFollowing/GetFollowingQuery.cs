namespace Application.Follows.Queries.GetFollowing;

public class GetFollowingQuery : IRequest<Result<List<UserViewModel>>>
{
    public string UserId { get; set; }
    public int PageNumber { get; set; }

    public GetFollowingQuery(string userId, int pageNumber)
    {
        UserId = userId;
        PageNumber = pageNumber;
    }
}
