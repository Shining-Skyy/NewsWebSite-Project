using Domain.Banners;

namespace Application.Banners.Dto
{
    public class BannerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public Position Position { get; set; }
        public int Priority { get; set; }
    }
}
