using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Order.API.RedisServices
{
    public class RedisService
    {
        private readonly IDatabase _database;
        private readonly ConnectionMultiplexer _redis;

        public RedisService(IConfiguration configuration)
        {
            var host = configuration.GetSection("Redis")["Host"];
            var port = configuration.GetSection("Redis")["Port"];
            _redis = ConnectionMultiplexer.Connect($"{host}:{port}");
            _database = _redis.GetDatabase(1);
        }
        public ConnectionMultiplexer GetConnectionMultiplexer => _redis;
        public async Task<bool> SetAsync<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            return await _database.StringSetAsync(key, json);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var json = await _database.StringGetAsync(key);
            return json.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }
    }
}
