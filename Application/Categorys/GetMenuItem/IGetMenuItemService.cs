using Application.Categorys.GetMenuItem.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categorys.GetMenuItem
{
    public interface IGetMenuItemService
    {
        List<MenuItemDto> Execute();
    }
}
