using Application.Posts.GetPostPLP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.HomePageService.Dto
{
    public class HomePageDto
    {
        public List<BannerDto> Banners { get; set; }
        public List<PostPLPDto> MostPopular { get; set; }
        public List<PostPLPDto> MostVisited { get; set; }
        public List<PostPLPDto> newest { get; set; }
    }
}
