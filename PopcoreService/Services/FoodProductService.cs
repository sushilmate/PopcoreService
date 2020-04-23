using Popcore.API.Models;
using Popcore.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Popcore.API.Services
{
    public class FoodProductService : IFoodProductService
    {
        private readonly IOpenFoodFactsProxyService _openFoodFactsProxyService;

        public FoodProductService(IOpenFoodFactsProxyService openFoodFactsProxyService)
        {
            _openFoodFactsProxyService = openFoodFactsProxyService;
        }

        public async Task<IEnumerable<FoodProductViewModel>> GetFoodProducts(string ingredient)
        {
            return await _openFoodFactsProxyService.GetFoodProducts(ingredient);
        }
    }
}
