using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Popcore.API.Models;
using Popcore.API.ViewModels;

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
        public async Task<IEnumerable<FoodProductViewModel>> GetAync(string ingredient)
        {
            if(string.IsNullOrWhiteSpace(ingredient))
            {
                return null;
            }

            return await _foodProductService.GetFoodProducts(ingredient);
            
            // mapper will map model to view model & client will get viewmodel from api.
            //return _mapper.Map<IEnumerable<Game>, IEnumerable<GameViewModel>>(games);
        }
    }
}
