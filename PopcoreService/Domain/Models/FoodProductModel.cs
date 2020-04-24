using System.Text.Json.Serialization;

namespace Popcore.API.Domain.Models
{


    public class FoodProductModel
    {
        public FoodProductModel()
        {

        }

        [JsonPropertyName("products")]
        public Product[] Products { get; set; }

    }
}
