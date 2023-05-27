namespace Application.Common.Models;

public class UserViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public bool IsCurrentUser { get; set; }
    public bool IsFollowing { get; set; }
    public bool IsBlocked { get; set; }
}
