using board_dotnet.Service;

using StackExchange.Redis;

namespace board_dotnet.Repository
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _redis;

        public RedisService(IDatabase redis)
        {
            _redis = redis;
        }

        public async Task<RedisValue> StringGet(string key)
        {
            return await _redis.StringGetAsync(key);
        }

        public async Task StringSet(string key, object value, TimeSpan? expiry = null)
        {
            await _redis.StringSetAsync(key, value.ToString(), expiry);
        }

        public async Task DeleteKey(string key)
        {
            await _redis.KeyDeleteAsync(key);
        }
    }
}