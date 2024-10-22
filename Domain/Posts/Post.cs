using Domain.Attributes;
using Domain.Categorys;

namespace Domain.Posts
{
    [Auditable]
    public class Post
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string PostDescription { get; set; }
        public string Content { get; set; }
        public int TimeRequired { get; set; }
        public string UserId { get; set; }
        public int VisitCount { get; set; }

        public int CategoryTypeId { get; set; }
        public virtual CategoryType CategoryType { get; set; }

        public virtual ICollection<PostImage> PostImages { get; set; }
        public virtual ICollection<PostFavorite> PostFavourites { get; set; }
    }
}
