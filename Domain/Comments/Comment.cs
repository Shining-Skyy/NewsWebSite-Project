using Domain.Attributes;
using Domain.Posts;

namespace Domain.Comments
{
    [Auditable]
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public int? ParentTypeId { get; set; }
        public Comment ParentType { get; set; }
        public ICollection<Comment> SubType { get; set; }
    }
}
