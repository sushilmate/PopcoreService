using System;

namespace Popcore.API.Providers
{
    public class CacheSetting
    {
        public DateTime ExpiresAt { get; set; }
        public int Value { get; set; }
    }
}
