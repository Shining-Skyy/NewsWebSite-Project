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

        public PaginatedItemsDto<PostPLPDto> Execute(int page, int pageSize)
        {
            int rowCount = 0;
            var data = context.Posts.Include(p => p.PostImages).OrderByDescending(p => p.Id)
                .PagedResult(page, pageSize, out rowCount).Select(p => new PostPLPDto
                {
                    Id = p.Id,
                    Titel = p.Titel,
                    Type = p.CategoryType.Type,
                    Image = uriComposer.ComposeImageUri(p.PostImages.FirstOrDefault().Src),
                }).ToList();
            return new PaginatedItemsDto<PostPLPDto>(page, pageSize, rowCount, data);
        }
    }
}
