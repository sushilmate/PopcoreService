using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Popcore.API.Logging;
using Popcore.API.Models;
using Popcore.API.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Popcore.API.Middlewares
{
    public class ApiRateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly object _syncLock = new object();
        private readonly IMemoryCache _cache;
        private readonly ICacheSettingProvider _cacheSettingProvider;
        private readonly ILogger<ApiRateLimitMiddleware> _logger;

        public ApiRateLimitMiddleware(RequestDelegate next, IMemoryCache cache, ICacheSettingProvider cacheSettingProvider, ILogger<ApiRateLimitMiddleware> logger)
        {
            _next = next;
            _cache = cache;
            _cacheSettingProvider = cacheSettingProvider;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsRateLimitRequestRequired(context))
            {
                _logger.LogWarning(LoggingMessages.Forbidden + context.User);

                await Task.Delay(3000);

                // reset to one as we wait for 3 seconds.
                CreateOrUpdateCache("RequestCount", _cacheSettingProvider.CreateCacheSetting(int.MaxValue));
            }
            await _next.Invoke(context);
        }

        private bool IsRateLimitRequestRequired(HttpContext context)
        {
            // lock to synchronise the multiple calls on web apis.
            lock (_syncLock)
            {
                if (!_cache.TryGetValue("RequestCount", out CacheSetting serviceHitCounter))
                {
                    // create service hit counter with 1
                    CreateOrUpdateCache("RequestCount", _cacheSettingProvider.CreateCacheSetting(int.MaxValue));
                    return false;
                }

                if(serviceHitCounter.Value == 3)
                {
                    return true;
                }

                serviceHitCounter.Value++;

                // updating the counter value
                CreateOrUpdateCache("RequestCount", serviceHitCounter);
         
                return false;
            }
        }

        public void CreateOrUpdateCache(string cacheName, CacheSetting cacheSetting)
        {
            // Set cache options.
            var cacheEntryOptions = _cacheSettingProvider.CreateMemoryCacheEntryOptions(CacheItemPriority.High, cacheSetting.ExpiresAt);

            // Save data in cache.
            _cache.Set(cacheName, cacheSetting, cacheEntryOptions);
        }
    }
}
