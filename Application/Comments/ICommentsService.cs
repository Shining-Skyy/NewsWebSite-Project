using Application.Comments.Dto;
using Application.Dtos;

namespace Application.Comments
{
    public interface ICommentsService
    {
        BaseDto<CommentDto> Add(CommentDto commentDto);
        List<CommentWithUserDto> GetList(int postId);
    }
}
