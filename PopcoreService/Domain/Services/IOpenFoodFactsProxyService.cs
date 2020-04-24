using Popcore.API.Domain.Models.Search.External;
using Popcore.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Popcore.API.Domain.Services
{
    public interface IOpenFoodFactsProxyService
    {
        Task<IEnumerable<ProductViewModel>> SearchFoodProducts(ProductSearchInput input);
    }
}