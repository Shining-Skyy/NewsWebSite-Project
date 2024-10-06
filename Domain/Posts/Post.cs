using Domain.Attributes;
using Domain.Categorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public CategoryType CategoryType { get; set; }

        public ICollection<PostImage> PostImages { get; set; }
        public ICollection<PostFavorite> PostFavourites { get; set; }
    }
}
