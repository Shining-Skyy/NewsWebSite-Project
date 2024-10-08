using Application.Banners.Dto;
using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Banners
{
    public interface IBannersService
    {
        PaginatedItemsDto<BannerDto> GetBanners(int pageIndex, int pageSize);
        void AddBanner(BannerDto banner);
    }
}
