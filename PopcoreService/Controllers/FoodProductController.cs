using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Popcore.API.Logging;
using Popcore.API.Models;
using System.Threading.Tasks;

namespace PopcoreService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodProductController : ControllerBase
    {
        private readonly IFoodProductService _foodProductService;
        private readonly IMapper _mapper;
        private readonly ILogger<FoodProductController> _logger;

        public FoodProductController(IFoodProductService foodProductService, IMapper mapper, ILogger<FoodProductController> logger)
        {
            _foodProductService = foodProductService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAync(string ingredient)
        {
            _logger.LogInformation(LoggingEvents.GetItem, LoggingMessages.GetAyncFoodProducts);

            if (string.IsNullOrWhiteSpace(ingredient))
            {
                _logger.LogInformation("Bad Input", LoggingMessages.GetAyncFoodProducts);

                return BadRequest("Input string ingredient is not valid");
            }

            var foodProducts = await _foodProductService.GetFoodProducts(ingredient);

            return Ok(foodProducts);

            // mapper will map model to view model & client will get viewmodel from api.
            //return _mapper.Map<IEnumerable<FoodProductModel>, IEnumerable<FoodProductViewModel>>(games);
        }
    }
}