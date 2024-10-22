using Application.Categorys.GetMenuItem.Dto;

namespace Application.Categorys.GetMenuItem
{
    public interface IGetMenuItemService
    {
        List<MenuItemDto> Execute();
    }
}
