namespace Domain.Entities;

[BsonCollection("User")]
public class User : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public int Followers { get; set; }
    public int Followings { get; set; }
    public bool IsBlockedByAdmin { get; set; }
    public DateTime? LastLogin { get; set; }
    public string? VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? PasswordResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
}
