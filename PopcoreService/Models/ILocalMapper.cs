using Popcore.API.Models.Entities;
using Popcore.API.ViewModels;
using System.Collections.Generic;

namespace Popcore.API.Models
{
    public interface ILocalMapper
    {
        ProductViewModel Map(Product product);
        List<ProductViewModel> Map(Product[] products);
    }
}