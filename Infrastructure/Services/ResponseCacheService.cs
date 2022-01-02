using System;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _redisDatabase;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _redisDatabase = redis.GetDatabase();
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if(response is null) return;

            var serializedResponse = JsonSerializer.Serialize(response, 
                new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            
            await _redisDatabase.StringSetAsync(cacheKey, serializedResponse, timeToLive);
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _redisDatabase.StringGetAsync(cacheKey);
            if(String.IsNullOrEmpty(cachedResponse)) return null;
            return cachedResponse;
        }
    }
}