using Application.Comments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Comments
{
    public class DeleteModel : PageModel
    {
        private readonly ICommentsService commentsService;
        public DeleteModel(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [BindProperty]
        public int CommentId { get; set; }

        public void OnGet(int Id)
        {
            CommentId = Id;
        }

        public IActionResult OnPost()
        {
            var result = commentsService.Remove(CommentId);

            if(result.IsSuccess)
            return RedirectToPage("Index");

            return BadRequest();
        }
    }
}
