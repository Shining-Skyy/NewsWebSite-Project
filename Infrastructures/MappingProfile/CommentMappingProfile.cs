using Application.Comments.Dto;
using AutoMapper;
using Domain.Comments;

namespace Infrastructures.MappingProfile
{
    public class CommentMappingProfile : Profile
    {
        // Constructor for the CommentMappingProfile class.
        public CommentMappingProfile()
        {
            // Create a mapping between the Comment entity and CommentDto.
            // ReverseMap() allows for bi-directional mapping (Comment to CommentDto and vice versa).
            CreateMap<Comment, CommentDto>().ReverseMap();

            // Create a mapping between the Comment entity and CommentListDto.
            // This is useful for scenarios where a list of comments is needed.
            CreateMap<Comment, CommentListDto>().ReverseMap();

            // Create a mapping between CommentListDto and CommentWithUserDto.
            // This mapping is useful for including user information along with the comment details.
            CreateMap<CommentListDto, CommentWithUserDto>().ReverseMap();
        }
    }
}
