using Application.Dtos;
using Application.Posts.PostServices.Dto;

namespace Application.Posts.PostServices
{
    public interface IPostService
    {
        List<ListCategoryTypeDto> GetCategoryType();
        PaginatedItemsDto<PostListDto> GetPostList(string UserId, int page, int pageSize);
        BaseDto<PostDto> FindById(int Id);
        BaseDto<PostDto> Edit(PostDto postDto);
        BaseDto Remove(int Id, string UserId);
    }
}
