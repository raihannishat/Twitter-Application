namespace Application.Common.Interfaces;

public interface IRedisTokenService
{
    Task SetKeywithTTL(string key, string value, int days);
    Task UpdateKey(string key, string value);
    Task<string> GetKey(string key);
}
