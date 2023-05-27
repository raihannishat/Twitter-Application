namespace Domain.Entities;
[BsonCollection("Follow")]
public class Follow : Entity
{
    public string FollowerId { get; set; } = string.Empty;
    public string FollowedId { get; set; } = string.Empty;
}
