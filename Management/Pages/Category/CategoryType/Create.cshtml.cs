using Application.Categorys.CategoryTypes;
using Application.Categorys.CategoryTypes.Dtos;
using AutoMapper;
using Management.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Category.CategoryType
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ICategoryTypeService service;
        private readonly IMapper mapper;
        public CreateModel(ICategoryTypeService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [BindProperty]
        public CategoryTypeViewModel categoryType { get; set; } = new CategoryTypeViewModel { };
        public List<string> Message { get; set; } = new List<string>();

        public void OnGet(int? parentId)
        {
            categoryType.ParentTypeId = parentId;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var model = mapper.Map<CategoryTypeDto>(categoryType);
                var result = service.Add(model);

                if (result.IsSuccess)
                {
                    return RedirectToPage("index", new { parentid = categoryType.ParentTypeId });
                }
                Message = result.Message;
            }
            return Page();
        }
    }
}
