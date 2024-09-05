using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.GetPostPLP.Dto
{
    public class PostPLPDto
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string PostDescription { get; set; }
        public string AuthorName { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
    }
}
