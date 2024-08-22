using Application.Categorys.GetMenuItem.Dto;
using Application.Interfaces.Contexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Categorys.GetMenuItem
{
    public class GetMenuItemService : IGetMenuItemService
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public GetMenuItemService(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public List<MenuItemDto> Execute()
        {
            var categoryType = context.CategoryTypes.Include(p => p.ParentType).ToList();
            var data = mapper.Map<List<MenuItemDto>>(categoryType);
            return data;
        }
    }
}
