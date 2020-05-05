using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Popcore.API.Domain.Services;
using Popcore.API.Infrastructure.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace PopcoreService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodProductController : ControllerBase
    {
        private readonly IFoodProductService _foodProductService;
        private readonly ILogger<FoodProductController> _logger;

        public FoodProductController(IFoodProductService foodProductService, ILogger<FoodProductController> logger)
        {
            _foodProductService = foodProductService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchProductsByIngredientAync(string ingredient)
        {
            _logger.LogInformation(LoggingEvents.GetItem, LoggingMessages.GetAyncFoodProducts);

            if (string.IsNullOrWhiteSpace(ingredient))
            {
                _logger.LogInformation("Bad Input", LoggingMessages.GetAyncFoodProducts);

                return BadRequest("Input string ingredient is not valid");
            }

            var foodProducts = await _foodProductService.SearchFoodProducts(ingredient);

            if(!foodProducts.Any())
            {
                return NotFound();
            }

            return Ok(foodProducts);
        }
    }
}