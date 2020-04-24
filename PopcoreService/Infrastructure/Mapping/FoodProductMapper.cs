using Popcore.API.Domain.Infrastructure;
using Popcore.API.Domain.Models;
using Popcore.API.ViewModels;
using System.Collections.Generic;

namespace Popcore.API.Infrastructure.Mapping
{
    public class FoodProductMapper : ILocalMapper
    {
        public FoodProductMapper()
        {
        }

        public ProductViewModel Map(Product product)
        {
            return new ProductViewModel()
            {
                ProductName = product.ProductName,
                Ingredients = product.Ingredients.Split(',')
            };
        }

        public List<ProductViewModel> Map(Product[] products)
        {
            var productsVM = new List<ProductViewModel>();

            foreach (var product in products)
            {
                productsVM.Add(Map(product));
            }
            return productsVM;
        }
    }
}