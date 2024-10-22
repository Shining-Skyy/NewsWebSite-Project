using Application.Comments.Dto;
using AutoMapper;
using Domain.Comments;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.MappingProfile
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<Comment, CommentDto>().ReverseMap();

            CreateMap<Comment, CommentListDto>().ReverseMap();

            CreateMap<CommentListDto, CommentWithUserDto>().ReverseMap();
        }
    }
}
