using System.ComponentModel.DataAnnotations;

namespace NewsWebSite.ViewModel.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Maximum should be 50 characters")]
        public string Content { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public int? ParentTypeId { get; set; }
    }
}
