using Microsoft.Extensions.Caching.Memory;
using Popcore.API.Providers;
using System;

namespace Popcore.API.Models
{
    public interface ICacheSettingProvider
    {
        CacheSetting CreateCacheSetting(int expiryInSeconds);

        MemoryCacheEntryOptions CreateMemoryCacheEntryOptions(CacheItemPriority cacheItemPriority, DateTime expiryDate);

        MemoryCacheEntryOptions CreateMemoryCacheEntryOptions(CacheItemPriority cacheItemPriority);
        
        CacheSetting CreateCacheSetting(int expiryInSeconds, int intialValue);
    }
}
