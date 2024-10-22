using Application.Dtos;
using Application.Posts.GetPostPLP.Dto;

namespace Application.Posts.GetPostPLP
{
    public interface IGetPostPLPService
    {
        PaginatedItemsDto<PostPLPDto> Execute(PostPLPRequestDto request);
    }
}
