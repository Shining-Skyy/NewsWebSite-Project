using Application.Dtos;
using Application.Posts.AddNewPost.Dto;

namespace Application.Posts.AddNewPost
{
    public interface IAddNewPostService
    {
        BaseDto<int> Execute(AddNewPostDto dto);
    }
}
