using Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Posts
{
    [Auditable]
    public class PostFavorite
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
