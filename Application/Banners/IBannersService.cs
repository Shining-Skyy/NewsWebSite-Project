using Application.Banners.Dto;
using Application.Dtos;

namespace Application.Banners
{
    public interface IBannersService
    {
        PaginatedItemsDto<BannerDto> GetBanners(int pageIndex, int pageSize);
        void AddBanner(BannerDto banner);
    }
}
