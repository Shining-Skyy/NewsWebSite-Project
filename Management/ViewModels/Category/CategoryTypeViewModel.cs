using System.ComponentModel.DataAnnotations;

namespace Management.ViewModels.Category
{
    public class CategoryTypeViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Name Category")]
        [Required]
        [MaxLength(50, ErrorMessage = "Maximum should be 100 characters")]
        public string Type { get; set; }
        public int? ParentTypeId { get; set; }
    }
}
