using Microsoft.Extensions.Caching.Memory;
using Popcore.API.Models;
using Popcore.API.Providers;
using Popcore.API.ViewModels;
using System.Collections.Generic;
using System.Linq;
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
            // not sure with web api url so used US food facts and search api.
            string url = string.Format("https://us.openfoodfacts.org/cgi/search.pl?action=process&tagtype_0=categories&tag_contains_0=contains&tag_0=breakfast_cereals&tagtype_1=ingredients&tag_contains_1=contains&tag_1={0}&additives=with&sort_by=unique_scans_n&page_size=20&axis_x=energy-kj&axis_y=products_n&download=on&format=csv", ingredient);

            var foodProducts = new List<FoodProductViewModel>();

            using (WebClient client = new WebClient())
            {
                var result = await client.DownloadStringTaskAsync(url);

                var resultOnNewLine = result.Split('\n');

                var resultNew = resultOnNewLine[0].Split('\t');

                var index = 0;
                var productIndex = 0;
                var ingredientIndex = 0;

                Dictionary<string, List<string>> foodDetails = new Dictionary<string, List<string>>();


                foreach (var col in resultNew)
                {
                    if (col == "product_name")
                    {
                        productIndex = index;
                    }
                    if (col == "ingredients_text")
                    {
                        ingredientIndex = index;
                    }

                    index++;
                    if (productIndex != 0 && ingredientIndex != 0)
                        break;
                }

                foreach (var item in resultOnNewLine)
                {
                    var data = item.Split('\t');

                    try
                    {
                        // check if we are accessing out of bound index from array
                        if (data.Length < ingredientIndex)
                            continue;

                        if (foodDetails.ContainsKey(data[productIndex]))
                        {
                            foodDetails[data[productIndex]].Add(data[ingredientIndex]);
                        }
                        else
                        {
                            foodDetails.Add(data[productIndex], new List<string>());
                        }
                    }
                    catch (System.Exception ex)
                    {
                        //todo error handling.
                    }
                }
                foreach (var item in foodDetails)
                {
                    foodProducts.Add(new FoodProductViewModel() { ProductName = item.Key, Ingredients = item.Value.ToArray() });
                }
            }

            if (!_cache.TryGetValue("PaginationCount", out CacheSetting paginationCounter))
            {
                // create service hit pagination counter with 1
                CreateOrUpdateCache("PaginationCount", _cacheSettingProvider.CreateCacheSetting(int.MaxValue, 0));

                paginationCounter = _cache.Get<CacheSetting>("PaginationCount");
            }

            if (foodProducts.Count < paginationCounter.Value)
            {
                paginationCounter.Value = 0;
            }

            var foodToReturn = foodProducts.Skip(paginationCounter.Value).Take(5);

            // increment the pagination counter by 5
            paginationCounter.Value += 5;

            // updating the counter value
            CreateOrUpdateCache("PaginationCount", paginationCounter);

            return foodToReturn;
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