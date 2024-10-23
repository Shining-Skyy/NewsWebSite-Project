using Application.Comments.Dto;
using Application.Posts.GetPostPDP.Dto;

namespace NewsWebSite.Models
{
    public class MainPageModel
    {
        // This property holds the details of a specific post, encapsulated in a PostPDPDto object.
        public PostPDPDto PostPDPDto { get; set; }

        // This property is a list that contains comments associated with the post.
        // Each comment is represented by a CommentWithUserDto object, which includes user information.
        public List<CommentWithUserDto> CommentListDtos { get; set; }
    }
}
