using System;
using System.Text.Json;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Interfaces;
using AspCqrs.Application.Common.Models;
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

        public async Task<(Result, T)> GetAsync<T>(string key)
        {
            try
            {
                var cachedString = await _database.StringGetAsync(key);

                if (cachedString.IsNullOrEmpty)
                    return (Result.Failure("EmptyCache", $"Value with Cache Key '{key}' is Empty."), default);

                return (Result.Success(), JsonSerializer.Deserialize<T>(cachedString));
            }
            catch (Exception e)
            {
                return (Result.Failure("CacheError", e.Message), default);
            }
        }

        public async Task<Result> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            try
            {
                var stringValue = JsonSerializer.Serialize(value);

                await _database.StringSetAsync(key, stringValue, expiry, When.NotExists);

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure("CacheError", e.Message);
            }
        }

        public async Task<(Result, T)> GetAsync<T>(string key, string field)
        {
            try
            {
                var cachedString = await _database.HashGetAsync(key, field);

                if (cachedString.IsNullOrEmpty)
                    return (Result.Failure("EmptyCache", $"Value with Cache Key '{key}' is Empty."), default);

                return (Result.Success(), JsonSerializer.Deserialize<T>(cachedString));
            }
            catch (Exception e)
            {
                return (Result.Failure("CacheError", e.Message), default);
            }
        }

        public async Task<Result> SetAsync<T>(string key, string field, T value, TimeSpan? expiry = null)

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

                if (!result) return Result.Failure("CacheError", "Failed to Execute Transaction.");

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure("CacheError", e.Message);
            }
        }

        public async Task<Result> RemoveAsync(string key)
        {
            try
            {
                await _database.KeyDeleteAsync(key);

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure("CacheError", e.Message);
            }
        }

        public async Task<Result> SetExpiryAsync(string key, TimeSpan expiry)
        {
            try
            {
                await _database.KeyExpireAsync(key, expiry);

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure("CacheError", e.Message);
            }
        }
    }
}