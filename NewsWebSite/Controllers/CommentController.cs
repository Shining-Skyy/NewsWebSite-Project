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
            // Store the postId in the session for later use
            HttpContext.Session.SetInt32("postId", postId);

            // Store the parentTypeId in the session; if it's null, use 0 as the default value
            HttpContext.Session.SetInt32("parentTypeId", parentTypeId.GetValueOrDefault());

            return View();
        }

        [HttpPost]
        public IActionResult AddComment(CommentDto commentDto)
        {
            // Retrieve the postId from the session and convert it to a string
            var postId = HttpContext.Session.GetInt32("postId").ToString();

            // Parse the postId string back to an integer and assign it to the commentDto
            commentDto.PostId = Int32.Parse(postId);

            // Retrieve the parentTypeId from the session
            var parentTypeId = HttpContext.Session.GetInt32("parentTypeId");

            if (parentTypeId == 0)
                commentDto.ParentTypeId = null;

            commentDto.UserId = ClaimUtility.GetUserId(User);

            commentsService.Add(commentDto);

            // Redirect to the Details action of the Post controller, passing the postId
            return LocalRedirect(Url.Action("Details", "Post", new { id = commentDto.PostId }));
        }
    }
}
