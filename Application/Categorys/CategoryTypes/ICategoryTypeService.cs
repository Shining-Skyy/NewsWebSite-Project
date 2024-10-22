using Application.Categorys.CategoryTypes.Dtos;
using Application.Dtos;

namespace Application.Categorys.CategoryTypes
{
    public interface ICategoryTypeService
    {
        BaseDto<CategoryTypeDto> Add(CategoryTypeDto categoryType);
        BaseDto Remove(int Id);
        BaseDto<CategoryTypeDto> Edit(CategoryTypeDto categoryType);
        BaseDto<CategoryTypeDto> FindById(int Id);
        PaginatedItemsDto<CategoryTypeListDto> GetList(int? parentId, int page, int pageSize);
    }
}
