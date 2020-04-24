using System.Text.Json.Serialization;

namespace Popcore.API.Domain.Models
{
    public class Product
    {
        public Product()
        {

        }

        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }

        [JsonPropertyName("ingredients_text")]
        public string Ingredients { get; set; }
    }
}
