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

            var foodProductsByIngredient = new List<FoodProductViewModel>();

            using (WebClient client = new WebClient())
            {
                var allProductsDetails = await client.DownloadStringTaskAsync(url);

                var productDetails = allProductsDetails.Split('\n');
                
                var productColumns = productDetails[0].Split('\t');

                var index = 0;
                var productIndex = 0;
                var ingredientIndex = 0;

                Dictionary<string, List<string>> foodDetails = new Dictionary<string, List<string>>();

                // this loop is to iterate with columns & find the indexes of products & ingredients
                // considering the index might change in future so we our code will not break.
                foreach (var col in productColumns)
                {
                    switch (col)
                    {
                        case "product_name":
                            productIndex = index;
                            break;
                        case "ingredients_text":
                            ingredientIndex = index;
                            break;
                    }

                    index++;

                    //break the loop since we found our indexes.
                    if (productIndex != 0 && ingredientIndex != 0)
                        break;
                }

                foreach (var product in productDetails.Skip(1))
                {
                    var productData = product.Split('\t');

                    try
                    {
                        // check if we are accessing out of bound index from array
                        if (productData.Length < ingredientIndex)
                            continue;

                        if (foodDetails.ContainsKey(productData[productIndex]))
                        {
                            foodDetails[productData[productIndex]].Add(productData[ingredientIndex]);
                        }
                        else
                        {
                            foodDetails.Add(productData[productIndex], new List<string>());
                        }
                    }
                    catch (System.Exception ex)
                    {
                        //todo error handling.
                    }
                }
                foreach (var item in foodDetails)
                {
                    foodProductsByIngredient.Add(new FoodProductViewModel() { ProductName = item.Key, Ingredients = item.Value.ToArray() });
                }
            }

            if (!_cache.TryGetValue("PaginationCount", out CacheSetting paginationCounter))
            {
                // create service hit pagination counter with 1
                CreateOrUpdateCache("PaginationCount", _cacheSettingProvider.CreateCacheSetting(int.MaxValue, 0));

                paginationCounter = _cache.Get<CacheSetting>("PaginationCount");
            }

            // this condition to reset the counter as we reached the end of food products.
            if (foodProductsByIngredient.Count < paginationCounter.Value)
            {
                paginationCounter.Value = 0;
            }

            // skipping the already served foo products & taking only 5
            var foodToReturn = foodProductsByIngredient.Skip(paginationCounter.Value).Take(5);

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