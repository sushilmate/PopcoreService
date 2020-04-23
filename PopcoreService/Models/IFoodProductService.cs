﻿using Popcore.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Popcore.API.Models
{
    public interface IFoodProductService
    {
        Task<IEnumerable<FoodProductViewModel>> GetFoodProducts(string ingredient);
    }
}