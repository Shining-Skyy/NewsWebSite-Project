using Application.Categorys.CategoryTypes.Dtos;
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
        }
    }
}
