namespace Application.Follows.Shared.Models;

public class FollowDto
{
    public string? Id { get; set; }
    public List<string>? FollowerList { get; set; }
    public List<string>? FollowingList { get; set; }
}
