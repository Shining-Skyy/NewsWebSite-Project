using Domain.Attributes;

namespace Domain.Posts
{
    [Auditable]
    public class PostFavorite
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
