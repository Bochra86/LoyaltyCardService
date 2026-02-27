using LoyaltyCard.Application.Constraints;
using LoyaltyCard.Application.Interfaces;  
using StackExchange.Redis;
using System.Text.Json;

namespace LoyaltyCard.Infrastructure.Redis;

public class RedisCache: ICacheService
{
    private readonly IDatabase _database;
    public RedisCache(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }
    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;
        
        return JsonSerializer.Deserialize<T>(value.ToString());
    }
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var jsonValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, jsonValue, expiration?? CacheTtl.Meduim);
    }       
}
