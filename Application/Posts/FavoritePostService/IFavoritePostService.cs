using Application.Dtos;
using Application.Posts.FavoritePostService.Dto;

namespace Application.Posts.FavoritePostService
{
    public interface IFavoritePostService
    {
        void AddFavorite(string UserId, int PostId);
        void DeleteFavorite(string UserId, int PostId);
        PaginatedItemsDto<FavoritePostDto> GetFavorite(string UserId, int page = 1, int pageSize = 10);
    }
}
