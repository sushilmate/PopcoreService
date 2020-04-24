using Microsoft.Extensions.Caching.Memory;
using Popcore.API.Infrastructure.Providers;
using System;

namespace Popcore.API.Domain.Infrastructure
{
    public interface ICacheSettingProvider
    {
        CacheSetting CreateCacheSetting(int expiryInSeconds);

        MemoryCacheEntryOptions CreateMemoryCacheEntryOptions(CacheItemPriority cacheItemPriority, DateTime expiryDate);

        MemoryCacheEntryOptions CreateMemoryCacheEntryOptions(CacheItemPriority cacheItemPriority);

        CacheSetting CreateCacheSetting(int expiryInSeconds, int intialValue);
    }
}
