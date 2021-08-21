using System;
using System.Threading.Tasks;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface ICacheService
    {
        Task<(bool result, TData data)> GetAsync<TData>(string key);
        
        Task<bool> SetAsync<TData>(string key, TData value, TimeSpan? expiry = null);

        Task<(bool result, TData data)> GetAsync<TData>(string key, string field);

        Task<bool> SetAsync<TData>(string key, string field, TData value, TimeSpan? expiry = null);
        
        Task<bool> RemoveAsync(string key);
        
        Task<bool> IsExistAsync(string key);
        
        Task<bool> IsExistAsync(string key, string field);

        Task<bool> SetExpiryAsync(string key, TimeSpan expiry);
    }
}