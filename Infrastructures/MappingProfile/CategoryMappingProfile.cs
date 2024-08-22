using Application.Categorys.CategoryTypes.Dtos;
using Application.Categorys.GetMenuItem.Dto;
using AutoMapper;
using Domain.Categorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.MappingProfile
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryType, CategoryTypeDto>().ReverseMap();

            CreateMap<CategoryType, CategoryTypeListDto>()
                .ForMember(dest => dest.SubTypeCount, option => option.MapFrom(src => src.SubType.Count));

            CreateMap<CategoryType, MenuItemDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentTypeId))
                .ForMember(dest => dest.SubMenu, opt => opt.MapFrom(src => src.SubType));
        }
    }
}
