using StackExchange.Redis;

namespace board_dotnet.Service;

public interface IRedisService
{
    Task<RedisValue> StringGet(string key);
    Task StringSet(string key, object value, TimeSpan? expiry);
    Task DeleteKey(string key);
}