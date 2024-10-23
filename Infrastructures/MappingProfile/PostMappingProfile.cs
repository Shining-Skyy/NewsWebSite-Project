using Application.Posts.AddNewPost.Dto;
using Application.Posts.PostServices.Dto;
using AutoMapper;
using Domain.Posts;

namespace Infrastructures.MappingProfile
{
    public class PostMappingProfile : Profile
    {
        // Constructor for the PostMappingProfile class
        public PostMappingProfile()
        {
            // Create a mapping configuration between Post and AddNewPostDto
            CreateMap<Post, AddNewPostDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PostImages)).ReverseMap();

            // Create a mapping configuration between Post and PostDto
            CreateMap<Post, PostDto>().ReverseMap();

            // Create a mapping configuration between PostImage and AddNewPostImageDto
            CreateMap<PostImage, AddNewPostImageDto>().ReverseMap();
        }
    }
}
