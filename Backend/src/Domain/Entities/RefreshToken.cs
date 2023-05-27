namespace Domain.Entities;

[BsonCollection("RefreshToken")]
public class RefreshToken : Entity
{
    public string Token { get; set; } = string.Empty;
    public string JwtId { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool Used { get; set; }
    public bool Invalidate { get; set; }
    public string UserId { get; set; } = string.Empty;
}
