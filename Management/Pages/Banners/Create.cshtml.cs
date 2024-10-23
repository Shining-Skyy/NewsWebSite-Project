using Application.Banners;
using Application.Banners.Dto;
using Infrastructures.ExternalApi.ImageServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Banners
{
    [Authorize(Roles = "Admin")]
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
            // Upload the banner image using the image upload service
            var result = imageUploadService
                .Upload(new List<IFormFile> { BannerImage });

            // Check if any images were successfully uploaded
            if (result.Count > 0)
            {
                Data.Image = result.FirstOrDefault();
                banners.AddBanner(Data);
            }

            return RedirectToPage("Index");
        }
    }
}
