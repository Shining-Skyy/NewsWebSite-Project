using Amazon.Runtime.Internal.Util;
using Application.Banners;
using Application.Banners.Dto;
using Infrastructures.ExternalApi.ImageServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Banners
{
    public class CreateModel : PageModel
    {
        private readonly IBannersService banners;
        private readonly IImageUploadService imageUploadService;
        public CreateModel(IBannersService banners, IImageUploadService imageUploadService)
        {
            this.banners = banners;
            this.imageUploadService = imageUploadService;
        }

        [BindProperty]
        public BannerDto Data { get; set; }
        [BindProperty]
        public IFormFile BannerImage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var result = imageUploadService.Upload(new List<IFormFile> { BannerImage });
            if (result.Count > 0)
            {
                Data.Image = result.FirstOrDefault();
                banners.AddBanner(Data);
            }
            return RedirectToPage("Index");
        }
    }
}
