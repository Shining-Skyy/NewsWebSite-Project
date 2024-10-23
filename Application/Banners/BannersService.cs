using Application.Banners.Dto;
using Application.Dtos;
using Application.Interfaces.Contexts;
using Application.UriComposer;
using Common;
using Domain.Banners;

namespace Application.Banners
{
    public class BannersService : IBannersService
    {
        private readonly IDataBaseContext context;
        private readonly IUriComposerService uriComposer;

        public BannersService(IDataBaseContext context, IUriComposerService uriComposer)
        {
            this.context = context;
            this.uriComposer = uriComposer;
        }

        // Adding a new Banner object to the Banners collection in the context
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

        public PaginatedItemsDto<BannerDto> GetBanners(int pageIndex, int pageSize)
        {
            int rowCount = 0;

            // Retrieve a paginated result of banners from the context
            var banners = context.Banners
                .PagedResult(pageIndex, pageSize, out rowCount)
                .ToList()
                .Select(p => new BannerDto
                {
                    Id = p.Id,
                    Image = uriComposer.ComposeImageUri(p.Image),
                    IsActive = p.IsActive,
                    Link = p.Link,
                    Name = p.Name,
                    Position = p.Position,
                    Priority = p.Priority,
                })
                .ToList();

            return new PaginatedItemsDto<BannerDto>(pageIndex, pageSize, rowCount, banners);
        }
    }
}
