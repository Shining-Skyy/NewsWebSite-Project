using Application.Posts.GetPostPDP;
using Application.Posts.GetPostPLP;
using Application.Posts.PostServices;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace NewsWebSite.Controllers
{
    public class PostController : Controller
    {
        private readonly IGetPostPLPService getPostPLP;
        private readonly IGetPostPDPService getPostPDP;
        public PostController(IGetPostPLPService getPostPLP, IGetPostPDPService getPostPDP)
        {
            this.getPostPLP = getPostPLP;
            this.getPostPDP = getPostPDP;
        }

        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var data = getPostPLP.Execute(page, pageSize);
            return View(data);
        }

        public IActionResult Details(int Id)
        {
            var data = getPostPDP.Execute(Id);
            return View(data);
        }
    }
}
