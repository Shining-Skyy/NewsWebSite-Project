using Application.Dtos;
using Application.Posts.FavoritePostService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.FavoritePostService
{
    public interface IFavoritePostService
    {
        void AddFavorite(string UserId, int PostId);
        void DeleteFavorite(string UserId, int PostId);
        PaginatedItemsDto<FavoritePostDto> GetFavorite(string UserId, int page = 1, int pageSize = 10);
    }
}
