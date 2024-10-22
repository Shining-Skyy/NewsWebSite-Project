using Domain.Banners;
using Domain.Categorys;
using Domain.Comments;
using Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces.Contexts
{
    public interface IDataBaseContext
    {
        DbSet<CategoryType> CategoryTypes { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<PostFavorite> PostFavorites { get; set; }
        DbSet<Banner> Banners { get; set; }
        DbSet<Comment> Comments { get; set; }

        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
