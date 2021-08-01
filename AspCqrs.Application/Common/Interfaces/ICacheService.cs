using System;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Models;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface ICacheService
    {
        Task<(Result, T)> GetAsync<T>(string key);
        
        Task<Result> SetAsync<T>(string key, T value, TimeSpan? expiry = null);

        Task<(Result, T)> GetAsync<T>(string key, string field);

        Task<Result> SetAsync<T>(string key, string field, T value, TimeSpan? expiry = null);
        
        Task<Result> RemoveAsync(string key);

        Task<Result> SetExpiryAsync(string key, TimeSpan expiry);
    }
}