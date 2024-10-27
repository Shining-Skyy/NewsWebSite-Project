using Application.Categorys.CategoryTypes.Dtos;
using Application.Comments.Dto;
using Application.Dtos;

namespace Application.Comments
{
    public interface ICommentsService
    {
        BaseDto<CommentDto> Add(CommentDto commentDto);
        List<CommentWithUserDto> GetListWhithId(int postId);
        PaginatedItemsDto<CommentListDto> GetList(int pageIndex, int pageSize);
        bool ChangeStatus(int commentId);
        BaseDto Remove(int Id);
        BaseDto<CommentDto> Edit(CommentDto categoryType);
    }
}
