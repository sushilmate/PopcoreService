using Popcore.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Popcore.API.Models
{
    public interface IFoodProductService
    {
        Task<IEnumerable<FoodProductViewModel>> GetFoodProducts(string ingredient);
    }
}
