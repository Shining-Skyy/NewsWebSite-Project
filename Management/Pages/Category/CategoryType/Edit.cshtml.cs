using Application.Categorys.CategoryTypes;
using Application.Categorys.CategoryTypes.Dtos;
using AutoMapper;
using Management.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Category.CategoryType
{
    public class EditModel : PageModel
    {
        private readonly ICategoryTypeService service;
        private readonly IMapper mapper;
        public EditModel(ICategoryTypeService service, IMapper mapper)
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
            else
            {
                Message = data.Message;
            }
        }

        public IActionResult OnPost()
        {
            var data = mapper.Map<CategoryTypeDto>(categoryType);
            var result = service.Edit(data);
            Message = result.Message;
            categoryType = mapper.Map<CategoryTypeViewModel>(result.Data);
            return Page();
        }
    }
}
