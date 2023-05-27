namespace Infrastructure.Configurations;

public class RedisSettings : IRedisSettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
