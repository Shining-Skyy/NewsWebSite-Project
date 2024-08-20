using Application.Categorys.CategoryTypes.Dtos;
using AutoMapper;
using Management.ViewModels.Category;

namespace Management.MappingProfiles
{
    public class CategoryVmMappingProfile : Profile
    {
        public CategoryVmMappingProfile()
        {
            CreateMap<CategoryTypeDto, CategoryTypeViewModel>().ReverseMap();
        }
    }
}
