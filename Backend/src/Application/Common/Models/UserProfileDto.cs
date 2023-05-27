namespace Application.Common.Models;

public class UserProfileDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime? VerifiedAt { get; set; }
    public int Followers { get; set; }
    public int Followings { get; set; }
    public bool IsFollowing { get; set; }
    public bool IsCurrentUserProfile { get; set; }
    public bool IsBlockedByCurrentUser { get; set; }
}
