using Application.Dtos;
using Application.Interfaces.Contexts;
using Application.Posts.GetPostPLP.Dto;
using Application.Posts.PostServices;
using Application.Posts.PostServices.Dto;
using Application.UriComposer;
using Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.GetPostPLP
{
    public class GetPostPLPService : IGetPostPLPService
    {
        private readonly IDataBaseContext context;
        private readonly IUriComposerService uriComposer;
        public GetPostPLPService(IDataBaseContext context, IUriComposerService uriComposer)
        {
            this.context = context;
            this.uriComposer = uriComposer;
        }

        public PaginatedItemsDto<PostPLPDto> Execute(PostPLPRequestDto request)
        {
            int rowCount = 0;
            var query = context.Posts.Include(p => p.PostImages).Include(p => p.CategoryType).AsQueryable();

            if (request.CategoryTypeId != null)
            {
                query = query.Where(p => p.CategoryTypeId == request.CategoryTypeId);
            }

            if (!string.IsNullOrEmpty(request.SearchKey))
            {
                query = query.Where(p => p.Titel.Contains(request.SearchKey) || p.Content.Contains(request.SearchKey));
            }

            if (request.SortType == SortType.MostVisited)
            {
                query = query.OrderByDescending(p => p.VisitCount);
            }

            if (request.SortType == SortType.MostPopular)
            {
                query = query.Include(p => p.PostFavourites)
                    .OrderByDescending(p => p.PostFavourites.Count());
            }

            if (request.SortType == SortType.newest)
            {
                query = query.OrderBy(p => p.Id);
            }

            var data = query.PagedResult(request.page, request.pageSize, out rowCount).ToList()
                .Select(p => new PostPLPDto()
                {
                    Id = p.Id,
                    Titel = p.Titel,
                    Type = p.CategoryType.Type,
                    Image = uriComposer.ComposeImageUri(p.PostImages.FirstOrDefault().Src),
                }).ToList();
            return new PaginatedItemsDto<PostPLPDto>(request.page, request.pageSize, rowCount, data);
        }
    }
}
