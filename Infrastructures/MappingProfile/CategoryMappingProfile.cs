using Application.Categorys.CategoryTypes.Dtos;
using Application.Categorys.GetMenuItem.Dto;
using AutoMapper;
using Domain.Categorys;

namespace Infrastructures.MappingProfile
{
    public class CategoryMappingProfile : Profile
    {
        // Constructor for the CategoryMappingProfile class
        public CategoryMappingProfile()
        {
            // Create a mapping between CategoryType and CategoryTypeDto
            // ReverseMap allows for two-way mapping between the two types
            CreateMap<CategoryType, CategoryTypeDto>().ReverseMap();

            // Create a mapping between CategoryType and CategoryTypeListDto
            CreateMap<CategoryType, CategoryTypeListDto>()
                .ForMember(dest => dest.SubTypeCount, option => option.MapFrom(src => src.SubType.Count));

            // Create a mapping between CategoryType and MenuItemDto
            CreateMap<CategoryType, MenuItemDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentTypeId))
                .ForMember(dest => dest.SubMenu, opt => opt.MapFrom(src => src.SubType));
        }
    }
}
