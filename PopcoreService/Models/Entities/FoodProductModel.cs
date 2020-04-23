using System.Text.Json.Serialization;

namespace Popcore.API.Models.Entities
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
