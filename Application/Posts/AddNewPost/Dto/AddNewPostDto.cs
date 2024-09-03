using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.AddNewPost.Dto
{
    public class AddNewPostDto
    {
        public string Titel { get; set; }
        public string PostDescription { get; set; }
        public string Content { get; set; }
        public int TimeRequired { get; set; }
        public string UserId { get; set; }
        public int CategoryTypeId { get; set; }

        public List<AddNewPostImageDto> Images { get; set; }
    }
}
