using Application.Categorys.CategoryTypes.Dtos;
using Application.Posts.PostServices.Dto;
using AutoMapper;
using Management.ViewModels.Category;
using Management.ViewModels.Post;

namespace Management.MappingProfiles
{
    public class ManagementVmMappingProfile : Profile
    {
        // This class defines a mapping profile for ViewModels and Data Transfer Objects (DTOs)
        public ManagementVmMappingProfile()
        {
            // Create a mapping between CategoryTypeDto and CategoryTypeViewModel
            CreateMap<CategoryTypeDto, CategoryTypeViewModel>().ReverseMap();

            // Create a mapping between PostDto and PostViewModel
            CreateMap<PostDto, PostViewModel>().ReverseMap();
        }
    }
}
