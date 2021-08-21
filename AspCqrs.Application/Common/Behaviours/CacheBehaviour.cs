using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Cache;
using AspCqrs.Application.Common.Interfaces;
using MediatR;

namespace AspCqrs.Application.Common.Behaviours
{
    public class CacheBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICacheService _cacheService;

        public CacheBehaviour(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var cachedAttribute = request.GetType().GetCustomAttribute<CachedAttribute>();

            if (cachedAttribute == null) return await next();

            var cacheField = GetCacheFieldFromRequest(request);

            var isExist = await _cacheService.IsExistAsync(cachedAttribute.Key, cacheField);


            if (isExist)
            {
                var (result, cachedData) = await _cacheService.GetAsync<TResponse>(cachedAttribute.Key, cacheField);
                
                if (result) return cachedData;
            }

            var response = await next();

            await _cacheService.SetAsync(cachedAttribute.Key, cacheField, response, TimeSpan.FromMinutes(10));

            return await next();
        }

        private static string GetCacheFieldFromRequest(TRequest request)
        {
            var properties = typeof(TRequest).GetProperties()
                .Where(info => info.GetValue(request, null) != null)
                .OrderBy(info => info.Name)
                .ToDictionary(info => info.Name, info => info.GetValue(request, null));

            var keyBuilder = new StringBuilder();
            keyBuilder.Append(nameof(TRequest));

            foreach (var (key, value) in properties)
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}