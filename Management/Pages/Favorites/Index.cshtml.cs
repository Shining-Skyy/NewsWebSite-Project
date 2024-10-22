using Application.Dtos;
using Application.Posts.FavoritePostService;
using Application.Posts.FavoritePostService.Dto;
using Management.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Favorites
{
    [Authorize(Roles = "Admin,Author")]
    public class IndexModel : PageModel
    {
        private readonly IFavoritePostService service;
        public IndexModel(IFavoritePostService service)
        {
            this.service = service;
        }

        public PaginatedItemsDto<FavoritePostDto> Post { get; set; }

        public void OnGet(int page = 1, int pageSize = 10)
        {
            Post = service.GetFavorite(ClaimUtility.GetUserId(User), page, pageSize);
        }

        public void OnPost(int postId)
        {
            service.DeleteFavorite(ClaimUtility.GetUserId(User), postId);
        }
    }
}
