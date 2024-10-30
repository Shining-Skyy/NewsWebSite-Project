using Application.Comments;
using Application.Comments.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebSite.Utilities;
using NLog;

namespace NewsWebSite.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentsService commentsService;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public CommentController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public IActionResult Index(int postId, int? parentTypeId)
        {
            // Store the postId in the session for later use
            HttpContext.Session.SetInt32("postId", postId);
            _logger.Info($"postId {postId} stored in session.");

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
            {
                _logger.Warn("parentTypeId is 0, setting ParentTypeId to null.");
                commentDto.ParentTypeId = null;
            }
            else
            {
                commentDto.ParentTypeId = parentTypeId;
                _logger.Debug($"Retrieved parentTypeId from session: {parentTypeId}");
            }

            commentsService.Add(commentDto);

            _logger.Info("Comment added successfully.");

            // Redirect to the Details action of the Post controller, passing the postId
            return LocalRedirect(Url.Action("Details", "Post", new { id = commentDto.PostId }));
        }
    }
}
