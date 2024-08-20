using Application.Categorys.CategoryTypes.Dtos;
using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
