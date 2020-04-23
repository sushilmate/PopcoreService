using Microsoft.Extensions.Caching.Memory;
using Popcore.API.Models;
using Popcore.API.Providers;
using Popcore.API.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Popcore.API.ThirdPartyProxyService
{
    public class OpenFoodFactsProxyService : IOpenFoodFactsProxyService
    {
        private readonly IMemoryCache _cache;
        private readonly ICacheSettingProvider _cacheSettingProvider;

        public OpenFoodFactsProxyService(IMemoryCache cache, ICacheSettingProvider cacheSettingProvider)
        {
            _cache = cache;
            _cacheSettingProvider = cacheSettingProvider;
        }

        public async Task<IEnumerable<FoodProductViewModel>> GetFoodProducts(string ingredient)
        {
            string url = "https://us.openfoodfacts.org/cgi/search.pl?action=process&tagtype_0=categories&tag_contains_0=contains&tag_0=breakfast_cereals&tagtype_1=nutrition_grades&tag_contains_1=contains&tag_1=A&additives=without&ingredients_from_palm_oil=without&sort_by=unique_scans_n&page_size=20&download=on&format=csv";

            using (WebClient client = new WebClient())
            {
                var result = await client.DownloadStringTaskAsync(url);
            }

            if (!_cache.TryGetValue("PaginationCount", out CacheSetting paginationCounter))
            {
                // create service hit pagination counter with 1
                CreateOrUpdateCache("PaginationCount", _cacheSettingProvider.CreateCacheSetting(int.MaxValue));

                paginationCounter = _cache.Get<CacheSetting>("PaginationCount");
            }

            // increment the pagination counter by 5
            paginationCounter.Value += 5;

            // updating the counter value
            CreateOrUpdateCache("PaginationCount", paginationCounter);

            return null;
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