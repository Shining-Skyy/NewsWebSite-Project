using Application.Posts.PostServices;
using Application.Posts.PostServices.Dto;
using AutoMapper;
using Management.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Post
{
    [Authorize(Roles = "Admin,Author")]
    public class EditModel : PageModel
    {
        private readonly IPostService service;
        private readonly IMapper mapper;
        public EditModel(IPostService service, IMapper mapper)
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
                // Map the retrieved data to the PostViewModel
                PostViewModel = mapper.Map<PostViewModel>(model.Data);

            Message = model.Message;
        }

        public IActionResult OnPost()
        {
            var model = mapper.Map<PostDto>(PostViewModel);
            var result = service.Edit(model);

            Message = result.Message;

            return RedirectToPage("Index");
        }
    }
}
