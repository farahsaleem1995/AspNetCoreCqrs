using System;
using System.Threading.Tasks;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

        Task<T> GetAsync<T>(string key, string field);

        Task SetAsync<T>(string key, string field, T value, TimeSpan? expiry = null);
        
        Task RemoveAsync(string key);

        Task SetExpiryAsync(string key, TimeSpan expiry);
    }
}