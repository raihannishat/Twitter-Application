namespace Application.Likes.Shared.Models;

public class LikeResponse
{
    public bool IsLikedByCurrentUser { get; set; }
    public int Likes { get; set; }
}
