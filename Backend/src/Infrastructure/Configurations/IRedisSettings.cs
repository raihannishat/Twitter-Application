namespace Infrastructure.Configurations;

public interface IRedisSettings
{
    string Endpoint { get; set; }
    string User { get; set; }
    string Password { get; set; }

}
