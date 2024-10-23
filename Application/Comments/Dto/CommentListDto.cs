namespace Application.Comments.Dto
{
    public class CommentListDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public int? ParentId { get; set; }
        public ICollection<CommentListDto> SubType { get; set; }
    }
}
