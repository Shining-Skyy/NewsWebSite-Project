using Domain.Attributes;

namespace Domain.Posts
{
    [Auditable]
    public class PostImage
    {
        public int Id { get; set; }
        public string Src { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
