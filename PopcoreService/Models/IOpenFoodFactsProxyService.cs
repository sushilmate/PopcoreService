using Popcore.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Popcore.API.Models
{
    public interface IOpenFoodFactsProxyService
    {
        Task<IEnumerable<FoodProductViewModel>> GetFoodProducts(string ingredient);
    }
}