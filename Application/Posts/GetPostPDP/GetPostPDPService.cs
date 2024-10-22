using Application.Interfaces.Contexts;
using Application.Posts.GetPostPDP.Dto;
using Application.UriComposer;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.GetPostPDP
{
    public class GetPostPDPService : IGetPostPDPService
    {
        private readonly IDataBaseContext dataBaseContext;
        private readonly IIdentityDatabaseContext identityDatabase;
        private readonly IUriComposerService uriComposer;
        public GetPostPDPService(IDataBaseContext dataBaseContext, IUriComposerService uriComposer,
            IIdentityDatabaseContext identityDatabase)
        {
            this.dataBaseContext = dataBaseContext;
            this.uriComposer = uriComposer;
            this.identityDatabase = identityDatabase;
        }

        public PostPDPDto Execute(int Id)
        {
            var post = dataBaseContext.Posts
                .Include(p => p.CategoryType)
                .Include(p => p.PostImages)
                .SingleOrDefault(p => p.Id == Id);

            var user = identityDatabase.Users.Where(p => p.Id == post.UserId).FirstOrDefault();

            post.VisitCount += 1;
            dataBaseContext.SaveChanges();

            var similarPost = dataBaseContext.Posts
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
                NameAuthor = user.FullName,
                similarPosts = similarPost,
            };
        }
    }
}
