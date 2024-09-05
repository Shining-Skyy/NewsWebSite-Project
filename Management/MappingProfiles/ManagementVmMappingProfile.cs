using Application.Categorys.CategoryTypes.Dtos;
using Application.Posts.PostServices.Dto;
using AutoMapper;
using Management.ViewModels.Category;
using Management.ViewModels.Post;

namespace Management.MappingProfiles
{
    public class ManagementVmMappingProfile : Profile
    {
        public ManagementVmMappingProfile()
        {
            CreateMap<CategoryTypeDto, CategoryTypeViewModel>().ReverseMap();
            CreateMap<PostDto, PostViewModel>().ReverseMap();
        }
    }
}
