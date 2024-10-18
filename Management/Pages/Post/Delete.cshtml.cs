using Application.Posts.PostServices;
using Application.Posts.PostServices.Dto;
using AutoMapper;
using Management.Utilities;
using Management.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Post
{
    [Authorize(Roles = "Admin,Author")]
    public class DeleteModel : PageModel
    {
        private readonly IPostService service;
        private readonly IMapper mapper;
        public DeleteModel(IPostService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [BindProperty]
        public PostViewModel PostViewModel { get; set; } = new PostViewModel();
        public List<string> Message { get; set; } = new List<string>();

        public void OnGet(int Id)
        {
            var model = service.FindById(Id);
            if (model.IsSuccess)
                PostViewModel = mapper.Map<PostViewModel>(model.Data);
            Message = model.Message;
        }

        public IActionResult OnPost()
        {
            var result = service.Remove(PostViewModel.Id, ClaimUtility.GetUserId(User));
            Message = result.Message;
            if (result.IsSuccess)
                return RedirectToPage("index");
            return Page();
        }
    }
}
