using Application.Posts.FavoritePostService;
using Application.Posts.GetPostPDP;
using Application.Posts.GetPostPLP;
using Application.Posts.GetPostPLP.Dto;
using Application.Posts.PostServices;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NewsWebSite.Controllers
{
    public class PostController : Controller
    {
        private readonly IGetPostPLPService getPostPLP;
        private readonly IGetPostPDPService getPostPDP;
        private readonly IFavoritePostService favoritePostService;

        public PostController(IGetPostPLPService getPostPLP, IGetPostPDPService getPostPDP,
            IFavoritePostService favoritePostService)
        {
            this.getPostPLP = getPostPLP;
            this.getPostPDP = getPostPDP;
            this.favoritePostService = favoritePostService;
        }

        public IActionResult Index(PostPLPRequestDto request)
        {
            var data = getPostPLP.Execute(request);
            return View(data);
        }

        public IActionResult Details(int Id)
        {
            var data = getPostPDP.Execute(Id);
            return View(data);
        }

        [Authorize]
        public IActionResult AddFavorite(int PostId)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            favoritePostService.AddFavorite(userId, PostId);
            return Redirect("https://localhost:44315/Favorites/Index");
        }
    }
}
