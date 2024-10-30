using Application.Comments;
using Application.Posts.FavoritePostService;
using Application.Posts.GetPostPDP;
using Application.Posts.GetPostPLP;
using Application.Posts.GetPostPLP.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebSite.Models;
using NewsWebSite.Utilities;
using NLog;

namespace NewsWebSite.Controllers
{
    public class PostController : Controller
    {
        private readonly IGetPostPLPService getPostPLP;
        private readonly IGetPostPDPService getPostPDP;
        private readonly IFavoritePostService favoritePostService;
        private readonly ICommentsService commentsService;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public PostController(IGetPostPLPService getPostPLP, IGetPostPDPService getPostPDP,
            IFavoritePostService favoritePostService, ICommentsService commentsService)
        {
            this.getPostPLP = getPostPLP;
            this.getPostPDP = getPostPDP;
            this.favoritePostService = favoritePostService;
            this.commentsService = commentsService;
        }

        public MainPageModel MainPageModel { get; set; } = new MainPageModel();

        public IActionResult Index(PostPLPRequestDto request)
        {
            var data = getPostPLP.Execute(request);
            if (data == null)
            {
                _logger.Warn("No data returned for request: {@Request}", request);
            }
            else
            {
                _logger.Info("Data successfully retrieved for request: {@Request}", request);
            }

            return View(data);
        }

        public IActionResult Details(int Id)
        {
            // Executes the method to retrieve a specific post based on its ID
            var dataPost = getPostPDP.Execute(Id);
            if (dataPost == null)
            {
                _logger.Warn($"No post found for Id: {Id}");
                return NotFound(); // Return a 404 if the post is not found
            }

            // Retrieves the list of comments associated with the post ID
            var dataComment = commentsService.GetListWhithId(Id);

            _logger.Info($"Comments retrieved for post Id: {Id}, Count: {dataComment.Count}");

            MainPageModel.PostPDPDto = dataPost;
            MainPageModel.CommentListDtos = dataComment;

            return View(MainPageModel);
        }

        [Authorize]
        public IActionResult AddFavorite(int PostId)
        {
            var userId = ClaimUtility.GetUserId(User);

            // Log the entry into the method with user ID and PostId
            _logger.Info($"User {userId} is attempting to add Post {PostId} to favorites.");

            favoritePostService.AddFavorite(userId, PostId);

            // Log successful addition of favorite
            _logger.Info($"Post {PostId} has been added to favorites by user {userId}.");

            return Redirect("https://localhost:44315/Favorites/Index");
        }
    }
}
