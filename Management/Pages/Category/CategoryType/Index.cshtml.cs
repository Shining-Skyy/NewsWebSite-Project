using Application.Categorys.CategoryTypes;
using Application.Categorys.CategoryTypes.Dtos;
using Application.Dtos;
using Management.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Category.CategoryType
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ICategoryTypeService service;
        public IndexModel(ICategoryTypeService service)
        {
            this.service = service;
        }

        public PaginatedItemsDto<CategoryTypeListDto> catalogType { get; set; } = null!;

        public void OnGet(int? parentId, int pageIndex = 1, int pageSize = 5)
        {
            catalogType = service.GetList(parentId, pageIndex, pageSize);
        }
    }
}
