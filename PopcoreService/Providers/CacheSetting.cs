using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Popcore.API.Providers
{
    public class CacheSetting
    {
        public DateTime ExpiresAt { get; set; }
        public int Value { get; set; }
    }
}
