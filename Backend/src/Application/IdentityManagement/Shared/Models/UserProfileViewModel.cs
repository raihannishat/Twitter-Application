namespace Application.IdentityManagement.Shared.Models;

public class UserProfileViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime? VerifiedAt { get; set; }
    public bool IsBlockedByAdmin { get; set; }
}