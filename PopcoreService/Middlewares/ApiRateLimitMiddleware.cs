﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Popcore.API.Domain.Infrastructure;
using Popcore.API.Infrastructure.Logging;
using Popcore.API.Infrastructure.Providers;
using System.Threading;
using System.Threading.Tasks;

namespace Popcore.API.Middlewares
{
    public class ApiRateLimitMiddleware
    {
        static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
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
            if (IsRateLimitRequestRequired())
            {
                _logger.LogWarning(LoggingMessages.Forbidden + context.User);

                // waiting for 3 seconds to serve the request.
                await semaphoreSlim.WaitAsync();
                try
                {
                    await Task.Delay(3000);
                }
                finally
                {
                    semaphoreSlim.Release();
                }

                // reset to one as we wait for 3 seconds.
                CreateOrUpdateCache("RequestCount", _cacheSettingProvider.CreateCacheSetting(int.MaxValue));
            }
            await _next.Invoke(context);
        }

        private bool IsRateLimitRequestRequired()
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

                // we hit the counter need to wait for 3 seconds.
                if (serviceHitCounter.Value == 3)
                {
                    return true;
                }

                // increment the counter since its not crossed the threshold.
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
