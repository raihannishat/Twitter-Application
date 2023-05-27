using StackExchange.Redis;

namespace Infrastructure.Services;

public class RedisTokenService : IRedisTokenService
{
    public readonly ConnectionMultiplexer connectionMultiplexer;
    private readonly IRedisSettings _redisSettings;

    public RedisTokenService(IRedisSettings redisSettings, IMapper mapper, IUserRepository userRepository)
    {
        _redisSettings = redisSettings;
        connectionMultiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            EndPoints = { _redisSettings.Endpoint },
            AbortOnConnectFail = false,
            User = _redisSettings.User,
            Password = _redisSettings.Password
        });
    }

    public async Task<string> GetKey(string key)
    {
        var db = connectionMultiplexer.GetDatabase();
        
        var res = await db.StringGetAsync(key);

        return res.ToString();
    }

    public async Task SetKeywithTTL(string key, string value, int days)
    {
        var db = connectionMultiplexer.GetDatabase();

        await db.StringSetAsync(key, value);

        await db.KeyExpireAsync(key, DateTime.UtcNow.AddDays(days));
    }

    public async Task UpdateKey(string key, string value)
    {
        var db = connectionMultiplexer.GetDatabase();

        var time = db.KeyTimeToLive(key);

        await db.StringSetAsync(key, value);

        await db.KeyExpireAsync(key, time);
    }
}
