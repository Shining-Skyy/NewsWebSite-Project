using Application.Banners;
using Application.Banners.Dto;
using Application.Dtos;
using Application.Posts.PostServices.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Banners
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IBannersService bannersService;
        public IndexModel(IBannersService bannersService)
        {
            this.bannersService = bannersService;
        }

        public PaginatedItemsDto<BannerDto> Dto { get; set; }

        public void OnGet(int pageIndex = 1, int pageSize = 5)
        {
            Dto = bannersService.GetBanners(pageIndex, pageSize);
        }
    }
}
