using System;

namespace Popcore.API.Infrastructure.Providers
{
    public class CacheSetting
    {
        public DateTime ExpiresAt { get; set; }
        public int Value { get; set; }
    }
}
