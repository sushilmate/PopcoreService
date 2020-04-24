using Popcore.API.Domain.Models.Search.External;
using Popcore.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Popcore.API.Domain.Services
{
    public interface IFoodProductService
    {
        Task<IEnumerable<ProductViewModel>> SearchFoodProducts(string searchTagValue,
            string searchTagType = "ingredients", SearchClause searchClause = SearchClause.Contains);
    }
}
