using Application.Categorys.GetMenuItem;
using Microsoft.AspNetCore.Mvc;

namespace NewsWebSite.Models.ViewComponents
{
    public class GetMenuCategories : ViewComponent
    {
        private readonly IGetMenuItemService service;
        public GetMenuCategories(IGetMenuItemService service)
        {
            this.service = service;
        }

        public IViewComponentResult Invoke()
        {
            var data = service.Execute();
            return View(viewName: "GetMenuCategories", model: data);
        }
    }
}
