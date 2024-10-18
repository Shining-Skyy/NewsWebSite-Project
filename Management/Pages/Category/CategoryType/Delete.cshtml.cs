using Application.Categorys.CategoryTypes;
using AutoMapper;
using Management.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Category.CategoryType
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ICategoryTypeService service;
        private readonly IMapper mapper;
        public DeleteModel(ICategoryTypeService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [BindProperty]
        public CategoryTypeViewModel categoryType { get; set; } = new CategoryTypeViewModel { };
        public List<string> Message { get; set; } = new List<string>();

        public void OnGet(int Id)
        {
            var data = service.FindById(Id);
            if (data.IsSuccess)
            {
                categoryType = mapper.Map<CategoryTypeViewModel>(data.Data);
            }
            Message = data.Message;
        }

        public IActionResult OnPost()
        {
            var result = service.Remove(categoryType.Id);
            Message = result.Message;
            if (result.IsSuccess)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
