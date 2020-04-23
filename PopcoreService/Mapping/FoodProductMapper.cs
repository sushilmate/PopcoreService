using Popcore.API.Models;
using Popcore.API.Models.Entities;
using Popcore.API.ViewModels;
using System.Collections.Generic;

namespace Popcore.API.Mapping
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