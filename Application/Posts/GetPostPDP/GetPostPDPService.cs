using Application.Interfaces.Contexts;
using Application.Posts.GetPostPDP.Dto;
using Application.UriComposer;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.GetPostPDP
{
    public class GetPostPDPService : IGetPostPDPService
    {
        private readonly IDataBaseContext context;
        private readonly IUriComposerService uriComposer;
        public GetPostPDPService(IDataBaseContext context, IUriComposerService uriComposer)
        {
            this.context = context;
            this.uriComposer = uriComposer;
        }

        public PostPDPDto Execute(int Id)
        {
            var post = context.Posts
                .Include(p => p.CategoryType)
                .Include(p => p.PostImages)
                .SingleOrDefault(p => p.Id == Id);

            post.VisitCount += 1;
            context.SaveChanges();

            var similarPost = context.Posts
                .Include(p => p.PostImages)
                .Where(p => p.CategoryTypeId == post.CategoryTypeId)
                .Take(5).Select(p => new SimilarPostDto
                {
                    Id = p.Id,
                    Titel = p.Titel,
                    TimeRequired = p.TimeRequired,
                    Image = uriComposer.ComposeImageUri(p.PostImages.FirstOrDefault().Src)
                }).ToList();

            return new PostPDPDto()
            {
                Id = post.Id,
                Titel = post.Titel,
                TimeRequired = post.TimeRequired,
                Content = post.Content,
                Type = post.CategoryType.Type,
                Image = post.PostImages.Select(p => uriComposer.ComposeImageUri(p.Src)).ToList(),
                similarPosts = similarPost,
            };
        }
    }
}
