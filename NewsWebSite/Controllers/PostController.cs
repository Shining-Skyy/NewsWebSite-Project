using Application.Comments;
using Application.Posts.FavoritePostService;
using Application.Posts.GetPostPDP;
using Application.Posts.GetPostPLP;
using Application.Posts.GetPostPLP.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsWebSite.Models;
using NewsWebSite.Utilities;

namespace NewsWebSite.Controllers
{
    public class PostController : Controller
    {
        private readonly IGetPostPLPService getPostPLP;
        private readonly IGetPostPDPService getPostPDP;
        private readonly IFavoritePostService favoritePostService;
        private readonly ICommentsService commentsService;

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
            return View(data);
        }

        public IActionResult Details(int Id)
        {
            // Executes the method to retrieve a specific post based on its ID
            var dataPost = getPostPDP.Execute(Id);

            // Retrieves the list of comments associated with the post ID
            var dataComment = commentsService.GetListWhithId(Id);

            MainPageModel.PostPDPDto = dataPost;
            MainPageModel.CommentListDtos = dataComment;

            return View(MainPageModel);
        }

        [Authorize]
        public IActionResult AddFavorite(int PostId)
        {
            favoritePostService.AddFavorite(ClaimUtility.GetUserId(User), PostId);
            return Redirect("https://localhost:44315/Favorites/Index");
        }
    }
}
