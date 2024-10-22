using Application.Comments.Dto;
using Application.Posts.GetPostPDP.Dto;

namespace NewsWebSite.Models
{
    public class MainPageModel
    {
        public PostPDPDto PostPDPDto { get; set; }
        public List<CommentWithUserDto> CommentListDtos { get; set; }
    }
}
