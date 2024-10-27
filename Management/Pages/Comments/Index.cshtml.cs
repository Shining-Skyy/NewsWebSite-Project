using Application.Comments;
using Application.Comments.Dto;
using Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Comments
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ICommentsService commentsService;
        public IndexModel(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }
        public PaginatedItemsDto<CommentListDto> CommentDto { get; set; }

        public void OnGet(int pageIndex = 1, int pageSize = 1)
        {
            CommentDto = commentsService.GetList(pageIndex, pageSize);
        }

        public IActionResult OnPost(int Id)
        {
            commentsService.ChangeStatus(Id);
            return RedirectToPage("Index");
        }
    }
}
