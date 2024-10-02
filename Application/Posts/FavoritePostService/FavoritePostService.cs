using Application.Dtos;
using Application.Interfaces.Contexts;
using Application.Posts.FavoritePostService.Dto;
using Application.UriComposer;
using Common;
using Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.FavoritePostService
{
    public class FavoritePostService : IFavoritePostService
    {
        private readonly IDataBaseContext context;
        private readonly IUriComposerService uriComposerService;
        public FavoritePostService(IDataBaseContext context, IUriComposerService uriComposerService)
        {
            this.context = context;
            this.uriComposerService = uriComposerService;
        }

        public void AddFavorite(string UserId, int PostId)
        {
            PostFavorite postFavorite = new PostFavorite
            {
                PostId = PostId,
                UserId = UserId,
            };
            context.PostFavorites.Add(postFavorite);
            context.SaveChanges();
        }

        public void DeleteFavorite(string UserId, int PostId)
        {
            var post = context.PostFavorites.Find(PostId);
            if (post.UserId == UserId)
            {
                context.PostFavorites.Remove(post);
                context.SaveChanges();
            }
        }

        public PaginatedItemsDto<FavoritePostDto> GetFavorite(string UserId, int page = 1, int pageSize = 10)
        {
            var model = context.Posts
               .Include(p => p.PostImages)
               .Include(p => p.PostFavourites)
               .Where(p => p.PostFavourites.Any(f => f.UserId == UserId))
               .OrderByDescending(p => p.Id)
               .AsQueryable();

            int rowCount = 0;
            var data = model.PagedResult(page, pageSize, out rowCount)
            .ToList()
            .Select(p => new FavoritePostDto
            {
                Id = p.Id,
                Titel = p.Titel,
                Image = uriComposerService.ComposeImageUri(p.PostImages.FirstOrDefault().Src),
            }).ToList();
            return new PaginatedItemsDto<FavoritePostDto>(page, pageSize, rowCount, data);
        }
    }
}
