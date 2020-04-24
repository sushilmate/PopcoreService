using Popcore.API.Domain.Models.Search.External;
using Popcore.API.Domain.Services;
using Popcore.API.ViewModels;
using System.Collections.Generic;
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

        public async Task<IEnumerable<ProductViewModel>> SearchFoodProducts(string searchTagValue, 
            string searchTagType = "ingredients", SearchClause searchClause = SearchClause.Contains)
        {
            var searchInput = new ProductSearchInput
            {
                CriteriaType = searchTagType,
                CriteriaClause = searchClause,
                CriteriaValue = searchTagValue
            };

            return await _openFoodFactsProxyService.SearchFoodProducts(searchInput);
        }
    }
}
