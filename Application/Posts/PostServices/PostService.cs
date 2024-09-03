using Application.Interfaces.Contexts;
using Application.Posts.PostServices.Dto;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.PostServices
{
    public class PostService : IPostService
    {
        private readonly IDataBaseContext context;
        public PostService(IDataBaseContext context)
        {
            this.context = context;
        }

        public List<ListCategoryTypeDto> GetCategoryType()
        {
            var data = context.CategoryTypes.Include(p => p.ParentType).ThenInclude(p => p.ParentType.ParentType)
                .Include(p => p.SubType).Where(p => p.ParentTypeId != null).Where(p => p.SubType.Count == 0)
                .Select(p => new { p.Id, p.Type, p.ParentType, p.SubType }).ToList()
                .Select(p => new ListCategoryTypeDto
                {
                    Id = p.Id,
                    Type = $"{p?.Type ?? ""} - {p?.ParentType?.Type ?? ""} - {p?.ParentType?.ParentType?.Type ?? ""}"
                }).ToList();
            return data;
        }
    }
}
