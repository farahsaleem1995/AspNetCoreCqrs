using System;
using System.Text.Json;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace AspCqrs.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly ILogger<RedisCacheService> _logger;
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer,
            ILogger<RedisCacheService> logger)
        {
            _logger = logger;
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<(bool result, TData data)> GetAsync<TData>(string key)
        {
            try
            {
                var cachedString = await _database.StringGetAsync(key);

                return cachedString.IsNullOrEmpty
                    ? (false, default)
                    : (true, JsonSerializer.Deserialize<TData>(cachedString));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to get cached value with key '{key}'.");
                
                return (false, default);
            }
        }

        public async Task<bool> SetAsync<TData>(string key, TData value, TimeSpan? expiry = null)
        {
            try
            {
                var stringValue = JsonSerializer.Serialize(value);

                await _database.StringSetAsync(key, stringValue, expiry, When.NotExists);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to set cache key '{key}'.");
                
                return false;
            }
        }

        public async Task<(bool result, TData data)> GetAsync<TData>(string key, string field)
        {
            try
            {
                var cachedString = await _database.HashGetAsync(key, field);

                return cachedString.IsNullOrEmpty
                    ? (false, default)
                    : (true, JsonSerializer.Deserialize<TData>(cachedString));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to get cached value with key '{key}'.");
                
                return (false, default);
            }
        }

        public async Task<bool> SetAsync<TData>(string key, string field, TData value, TimeSpan? expiry = null)

        {
            try
            {
                var stringValue = JsonSerializer.Serialize(value);

                var transaction = _database.CreateTransaction();

                var result = await Task.Run(() =>
                {
                    transaction.HashSetAsync(key, field, stringValue, When.NotExists);

                    if (expiry != null) transaction.KeyExpireAsync(key, expiry);

                    return transaction.ExecuteAsync();
                });

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to set cache key '{key}'.");
                
                return false;
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                await _database.KeyDeleteAsync(key);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to remove cache key '{key}'.");
                
                return false;
            }
        }

        public async Task<bool> IsExistAsync(string key)
        {
            try
            {
                return await _database.KeyExistsAsync(key);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to check if cache key '{key}' exists.");
                
                return false;
            }
        }

        public async Task<bool> IsExistAsync(string key, string field)
        {
            try
            {
                return await _database.HashExistsAsync(key, field);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to check if cache key '{key}' field '{field}' exists.");
                
                return false;
            }
        }

        public async Task<bool> SetExpiryAsync(string key, TimeSpan expiry)
        {
            try
            {
                await _database.KeyExpireAsync(key, expiry);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to set expiration date of cache key '{key}'.");
                
                return false;
            }
        }
    }
}