namespace Domain.Entities;

[BsonCollection("Block")]
public class Blocks : Entity
{
    public string BlockedId { get; set; } = string.Empty;
    public string BlockedById { get; set; } = string.Empty;
}
