using Application.Posts.GetPostPDP.Dto;

namespace Application.Posts.GetPostPDP
{
    public interface IGetPostPDPService
    {
        PostPDPDto Execute(int Id);
    }
}
