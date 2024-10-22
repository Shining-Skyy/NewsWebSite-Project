using Application.Visitors.VisitorOnline;
using Application.Visitors.VisitorOnline.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.OnlineUsers
{
    [Authorize(Roles = "Admin")]
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
