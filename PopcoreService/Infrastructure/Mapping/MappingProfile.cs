using AutoMapper;
using Popcore.API.Domain.Models;
using Popcore.API.ViewModels;
using System;

namespace Popcore.API.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(vm => vm.Ingredients, m => m.MapFrom(src => src.Ingredients.Split(',', StringSplitOptions.RemoveEmptyEntries)));
        }
    }
}