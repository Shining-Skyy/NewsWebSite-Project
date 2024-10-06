using Application.Banners.Dto;
using Application.Interfaces.Contexts;
using Domain.Banners;

namespace Application.Banners
{
    public class BannersService : IBannersService
    {
        private readonly IDataBaseContext context;

        public BannersService(IDataBaseContext context)
        {
            this.context = context;
        }

        public void AddBanner(BannerDto banner)
        {
            context.Banners.Add(new Banner
            {
                Image = banner.Image,
                IsActive = banner.IsActive,
                Link = banner.Link,
                Name = banner.Name,
                Position = banner.Position,
                Priority = banner.Priority,
            });
            context.SaveChanges();
        }

        public List<BannerDto> GetBanners()
        {
            var banners = context.Banners.Select(p => new BannerDto
            {
                Image = p.Image,
                IsActive = p.IsActive,
                Link = p.Link,
                Name = p.Link,
                Position = p.Position,
                Priority = p.Priority,
            }).ToList();
            return banners;
        }
    }
}
