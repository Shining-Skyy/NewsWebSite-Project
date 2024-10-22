using Application.Comments;
using Application.Comments.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebSite.Utilities;

namespace NewsWebSite.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentsService commentsService;
        public CommentController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public IActionResult Index(int postId, int? parentTypeId)
        {
            HttpContext.Session.SetInt32("postId", postId);
            HttpContext.Session.SetInt32("parentTypeId", parentTypeId.GetValueOrDefault());
            return View();
        }

        [HttpPost]
        public IActionResult AddComment(CommentDto commentDto)
        {
            var postId = HttpContext.Session.GetInt32("postId").ToString();
            commentDto.PostId = Int32.Parse(postId);

            var parentTypeId = HttpContext.Session.GetInt32("parentTypeId");
            if (parentTypeId == 0)
                commentDto.ParentTypeId = null;

            commentDto.UserId = ClaimUtility.GetUserId(User);

            commentsService.Add(commentDto);
            return LocalRedirect(Url.Action("Details", "Post", new { id = commentDto.PostId }));
        }
    }
}
