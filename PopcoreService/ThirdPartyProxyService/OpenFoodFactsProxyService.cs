using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Popcore.API.Models;
using Popcore.API.Models.Entities;
using Popcore.API.Providers;
using Popcore.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Popcore.API.ThirdPartyProxyService
{
    public class OpenFoodFactsProxyService : IOpenFoodFactsProxyService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILocalMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ICacheSettingProvider _cacheSettingProvider;
        private readonly ILogger<OpenFoodFactsProxyService> _logger;

        public OpenFoodFactsProxyService(IHttpClientFactory clientFactory, ILocalMapper mapper,
            IMemoryCache cache, ICacheSettingProvider cacheSettingProvider, ILogger<OpenFoodFactsProxyService> logger)
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
            _cache = cache;
            _cacheSettingProvider = cacheSettingProvider;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductViewModel>> GetFoodProducts(string ingredient)
        {
            // not sure with web api url so used US food facts and search api.
            string baseUrl = "https://us.openfoodfacts.org/cgi/search.pl?";
            string url = string.Format("{0}action=process&tagtype_0=ingredients&tag_contains_0=contains&tag_0={1}&json=true", baseUrl, ingredient);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // create http client for downloading the food products
            var client = _clientFactory.CreateClient();
            // timeout 20 seconds 
            client.Timeout = TimeSpan.FromSeconds(20);

            try
            {
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return Enumerable.Empty<ProductViewModel>();

                var responseStream = await response.Content.ReadAsStringAsync();

                var productDetails = JsonSerializer.Deserialize<FoodProductModel>(responseStream);

                // validate if the product details is null or 0 or no products
                if (productDetails == null || productDetails.Products == null || productDetails.Products.Length == 0)
                    return Enumerable.Empty<ProductViewModel>();

                var foodProductsByIngredient = _mapper.Map(productDetails.Products);

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
            catch (OperationCanceledException ex)
            {
                //todo third party api is taking too much time.
                _logger.LogError(ex.Message, request);
                return Enumerable.Empty<ProductViewModel>();
            }
            catch (Exception ex)
            {
                //todo third party api is taking too much time.
                _logger.LogError(ex.Message, request);
                return Enumerable.Empty<ProductViewModel>();
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