using System;
using System.Text.Json;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using StackExchange.Redis;

namespace AspCqrs.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var cachedString = await _database.StringGetAsync(key);

            return JsonSerializer.Deserialize<T>(cachedString);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var stringValue = JsonSerializer.Serialize(value);

            await _database.StringSetAsync(key, stringValue, expiry, When.NotExists);
        }

        public async Task<T> GetAsync<T>(string key, string field)
        {
            var cachedString = await _database.HashGetAsync(key, field);

            return JsonSerializer.Deserialize<T>(cachedString);
        }

        public async Task SetAsync<T>(string key, string field, T value, TimeSpan? expiry = null)

        {
            var stringValue = JsonSerializer.Serialize(value);

            var transaction = _database.CreateTransaction();

            var result = await Task.Run(() =>
            {
                transaction.HashSetAsync(key, field, stringValue, When.NotExists);

                if (expiry != null) transaction.KeyExpireAsync(key, expiry);

                return transaction.ExecuteAsync();
            });

            if (!result) throw new Exception($"Failed to cache key {key}");
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task SetExpiryAsync(string key, TimeSpan expiry)
        {
            await _database.KeyExpireAsync(key, expiry);
        }
    }
}