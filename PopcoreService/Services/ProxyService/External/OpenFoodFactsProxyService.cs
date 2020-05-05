using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Popcore.API.Domain.Infrastructure;
using Popcore.API.Domain.Models;
using Popcore.API.Domain.Models.Search.External;
using Popcore.API.Domain.Services;
using Popcore.API.Infrastructure.Providers;
using Popcore.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Popcore.API.Services.ProxyService.External
{
    public class OpenFoodFactsProxyService : IOpenFoodFactsProxyService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ICacheSettingProvider _cacheSettingProvider;
        private readonly IQueryBuilder _httpQueryBuilder;
        private readonly ILogger<OpenFoodFactsProxyService> _logger;

        public OpenFoodFactsProxyService(IHttpClientFactory clientFactory, IMapper mapper,
            IMemoryCache cache, ICacheSettingProvider cacheSettingProvider, IQueryBuilder httpQueryBuilder, ILogger<OpenFoodFactsProxyService> logger)
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
            _cache = cache;
            _cacheSettingProvider = cacheSettingProvider;
            _httpQueryBuilder = httpQueryBuilder;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductViewModel>> SearchFoodProducts(ProductSearchInput input)
        {
            string url = _httpQueryBuilder.Build(input);

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
                if (!IsValid(productDetails))
                    return Enumerable.Empty<ProductViewModel>();

                if (!_cache.TryGetValue("PaginationCount", out CacheSetting paginationCounter))
                {
                    // create service hit pagination counter with 1
                    CreateOrUpdateCache("PaginationCount", _cacheSettingProvider.CreateCacheSetting(int.MaxValue, 0));

                    paginationCounter = _cache.Get<CacheSetting>("PaginationCount");
                }

                // this condition to reset the counter as we reached the end of food products.
                if (productDetails.Products.Length < paginationCounter.Value)
                {
                    paginationCounter.Value = 0;
                }

                // skipping the already served foo products & taking only 5
                var foodToReturn = productDetails.Products.Skip(paginationCounter.Value).Take(5);

                // increment the pagination counter by 5
                paginationCounter.Value += 5;

                // updating the counter value
                CreateOrUpdateCache("PaginationCount", paginationCounter);

                return _mapper.Map<List<ProductViewModel>>(foodToReturn);
            }
            catch (Exception ex)
            {
                //todo third party api is taking too much time.
                _logger.LogError(ex.Message, request);
            }
            return Enumerable.Empty<ProductViewModel>();
        }

        private bool IsValid(FoodProductModel productDetails)
        {
            return productDetails == null || productDetails.Products == null || productDetails.Products.Length == 0;
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