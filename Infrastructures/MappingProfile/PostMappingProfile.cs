using Application.Posts.AddNewPost.Dto;
using Application.Posts.PostServices.Dto;
using AutoMapper;
using Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.MappingProfile
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            CreateMap<Post, AddNewPostDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PostImages)).ReverseMap();

            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<PostImage, AddNewPostImageDto>().ReverseMap();
        }
    }
}
