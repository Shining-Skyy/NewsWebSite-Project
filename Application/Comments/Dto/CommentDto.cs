namespace Application.Comments.Dto
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public int? ParentTypeId { get; set; }
    }
}
