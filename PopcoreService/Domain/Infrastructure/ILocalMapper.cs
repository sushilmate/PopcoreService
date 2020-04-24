using Popcore.API.Domain.Models;
using Popcore.API.ViewModels;
using System.Collections.Generic;

namespace Popcore.API.Domain.Infrastructure
{
    public interface ILocalMapper
    {
        ProductViewModel Map(Product product);
        List<ProductViewModel> Map(Product[] products);
    }
}