using Application.Dtos;
using Application.Posts.AddNewPost;
using Application.Posts.AddNewPost.Dto;
using Application.Posts.PostServices;
using Domain.Users;
using Infrastructures.ExternalApi.ImageServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Management.Pages.Post
{
    [Authorize(Roles = "Admin,Author")]
    public class CreateModel : PageModel
    {
        private readonly IAddNewPostService addNewPostService;
        private readonly IPostService postService;
        private readonly IImageUploadService uploadService;
        private readonly UserManager<User> _userManager;
        public CreateModel(IAddNewPostService addNewPostService, IPostService postService,
            IImageUploadService uploadService, UserManager<User> userManager)
        {
            this.addNewPostService = addNewPostService;
            this.postService = postService;
            this.uploadService = uploadService;
            _userManager = userManager;
        }

        public SelectList Categories { get; set; }
        [BindProperty]
        public AddNewPostDto Data { get; set; }
        [BindProperty]
        public List<IFormFile> Files { get; set; }

        public void OnGet()
        {
            Categories = new SelectList(postService.GetCategoryType(), "Id", "Type");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new JsonResult(new BaseDto<int>(0, allErrors.Select(p => p.ErrorMessage).ToList(), false));
            }

            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                Files.Add(file);
            }

            List<AddNewPostImageDto> images = new List<AddNewPostImageDto>();
            if (Files.Count > 0)
            {
                //Upload 
                var result = uploadService.Upload(Files);
                foreach (var item in result)
                {
                    images.Add(new AddNewPostImageDto { Src = item });
                }
            }
            Data.Images = images;
            var user = _userManager.GetUserAsync(User).Result;
            Data.UserId = user.Id;

            var resultService = addNewPostService.Execute(Data);

            return new JsonResult(resultService);
        }
    }
}
