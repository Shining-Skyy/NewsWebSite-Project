using Application.Posts.GetPostPLP.Dto;

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
