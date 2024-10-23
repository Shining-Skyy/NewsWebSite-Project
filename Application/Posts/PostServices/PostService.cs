using Application.Dtos;
using Application.Interfaces.Contexts;
using Application.Posts.PostServices.Dto;
using Application.UriComposer;
using AutoMapper;
using Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.PostServices
{
    public class PostService : IPostService
    {
        private readonly IDataBaseContext context;
        private readonly IUriComposerService uriComposer;
        private readonly IMapper mapper;
        public PostService(IDataBaseContext context, IUriComposerService uriComposer, IMapper mapper)
        {
            this.context = context;
            this.uriComposer = uriComposer;
            this.mapper = mapper;
        }

        public List<ListCategoryTypeDto> GetCategoryType()
        {
            // Fetching category types from the database context
            var data = context.CategoryTypes
                .Include(p => p.ParentType)
                .ThenInclude(p => p.ParentType.ParentType)
                .Include(p => p.SubType)
                .Where(p => p.ParentTypeId != null)
                .Where(p => p.SubType.Count == 0)
                .Select(p => new { p.Id, p.Type, p.ParentType, p.SubType })
                .ToList()
                .Select(p => new ListCategoryTypeDto
                {
                    Id = p.Id,
                    // Constructing a string that includes the Type and its Parent Types, handling nulls
                    Type = $"{p?.Type ?? ""} - {p?.ParentType?.Type ?? ""} - {p?.ParentType?.ParentType?.Type ?? ""}"
                })
                .ToList();

            return data;
        }

        public BaseDto<PostDto> FindById(int Id)
        {
            var data = context.Posts.Find(Id);

            // Mapping the found post to PostDto
            var result = mapper.Map<PostDto>(data);

            return new BaseDto<PostDto>(result, null, true);
        }

        public BaseDto<PostDto> Edit(PostDto postDto)
        {
            // Finding the post that matches the UserId and Id from the provided PostDto
            var model = context.Posts
                .Where(p => p.UserId == postDto.UserId)
                .SingleOrDefault(p => p.Id == postDto.Id);

            mapper.Map(postDto, model);
            context.SaveChanges();

            return new BaseDto<PostDto>(mapper.Map<PostDto>(model), new List<string> { $"Edited successfully" }, true);
        }

        public BaseDto Remove(int Id, string UserId)
        {
            var data = context.Posts.Find(Id);

            // Checking if the UserId matches the post's UserId for deletion permission
            if (data.UserId == UserId)
            {
                context.Posts.Remove(data);
                context.SaveChanges();
                return new BaseDto(new List<string> { $"Item deleted successfully" }, true);
            }
            else
            {
                return new BaseDto(new List<string> { $"The deletion failed" }, false);
            }
        }

        public PaginatedItemsDto<PostListDto> GetPostList(string UserId, int page, int pageSize)
        {
            int rowCount = 0;

            // Fetching a paginated list of posts for the specified UserId
            var data = context.Posts
                .Include(p => p.PostImages)
                .ToPaged(page, pageSize)
                .OrderByDescending(p => p.Id)
                .Where(p => p.UserId == UserId)
                .Select(p => new PostListDto
                {
                    Id = p.Id,
                    Titel = p.Titel,
                    Image = uriComposer.ComposeImageUri(p.PostImages.FirstOrDefault().Src),
                })
                .ToList();

            return new PaginatedItemsDto<PostListDto>(page, pageSize, rowCount, data);
        }
    }
}
