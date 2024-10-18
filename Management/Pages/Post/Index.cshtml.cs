using Application.Dtos;
using Application.Posts.PostServices;
using Application.Posts.PostServices.Dto;
using Management.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using System.Security.Claims;

namespace Management.Pages.Post
{
    [Authorize(Roles = "Admin,Author")]
    public class IndexModel : PageModel
    {
        private readonly IPostService service;
        public IndexModel(IPostService service)
        {
            this.service = service;
        }

        public PaginatedItemsDto<PostListDto> Post { get; set; }

        public void OnGet(int page = 1, int pageSize = 10)
        {
            Post = service.GetPostList(ClaimUtility.GetUserId(User), page, pageSize);
        }
    }
}
