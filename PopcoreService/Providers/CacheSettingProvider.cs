using Microsoft.Extensions.Caching.Memory;
using Popcore.API.Models;
using System;

namespace Popcore.API.Providers
{
    public class CacheSettingProvider : ICacheSettingProvider
    {
        public CacheSetting CreateCacheSetting(int expiryInSeconds)
        {
            return new CacheSetting()
            {
                ExpiresAt = DateTime.Now.AddSeconds(expiryInSeconds),
                Value = 1
            };
        }

        public CacheSetting CreateCacheSetting(int expiryInSeconds, int intialValue)
        {
            return new CacheSetting()
            {
                ExpiresAt = DateTime.Now.AddSeconds(expiryInSeconds),
                Value = intialValue
            };
        }

        //public CacheSetting CreateCacheSetting(CacheItemPriority cacheItemPriority)
        //{
        //    return new CacheSetting()
        //    {
        //        Value = 1
        //    };
        //}

        public MemoryCacheEntryOptions CreateMemoryCacheEntryOptions(CacheItemPriority cacheItemPriority, DateTime expiryDate)
        {
            return new MemoryCacheEntryOptions()
            {
                Priority = cacheItemPriority,
                AbsoluteExpiration = expiryDate
            };
        }

        public MemoryCacheEntryOptions CreateMemoryCacheEntryOptions(CacheItemPriority cacheItemPriority)
        {
            return new MemoryCacheEntryOptions()
            {
                Priority = cacheItemPriority
            };
        }

    }
}
