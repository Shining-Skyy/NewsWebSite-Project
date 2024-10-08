using Application.Visitors.VisitorOnline;
using Application.Visitors.VisitorOnline.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.OnlineUsers
{
    public class IndexModel : PageModel
    {
        private readonly IVisitorOnlineService _visitorOnlineService;
        public VisitorOnlineDto VisitorOnlineDto;
        public IndexModel(IVisitorOnlineService visitorOnlineService)
        {
            _visitorOnlineService = visitorOnlineService;
        }

        public void OnGet()
        {
            VisitorOnlineDto = _visitorOnlineService.GetAll();
        }
    }
}
