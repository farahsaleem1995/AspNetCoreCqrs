using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspCqrs.Application.Common.Cache;
using AspCqrs.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AspCqrs.Application.Common.Behaviours
{
    public class CacheBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<CacheBehaviour<TRequest, TResponse>> _logger;

        public CacheBehaviour(ICacheService cacheService,
            ILogger<CacheBehaviour<TRequest, TResponse>> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var cachedAttributes = request.GetType().GetCustomAttribute<CachedAttribute>();

            if (cachedAttributes == null) return await next();
            
            var cacheField = GetCacheFieldFromRequest(request);

            try
            {
                return await _cacheService.GetAsync<TResponse>(cachedAttributes.Key, cacheField);
            }
            catch (Exception e)
            {
                var requestName = typeof(TRequest).Name;
                    
                _logger.LogWarning(e, "Failed to Get Data from Cache Memory for Request {Name} {@Request}",
                    requestName, request);
            }

            var response = await next();
            
            try
            {
                await _cacheService.SetAsync(cachedAttributes.Key, cacheField, response, TimeSpan.FromMinutes(10));
            }
            catch (Exception e)
            {
                var requestName = typeof(TRequest).Name;
                    
                _logger.LogWarning(e, "Failed to Set Data to Cache Memory for Request {Name} {@Request}",
                    requestName, request);
            }

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