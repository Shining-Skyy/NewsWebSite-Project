using Application.Dtos;
using Application.Interfaces.Contexts;
using Application.Posts.AddNewPost.Dto;
using AutoMapper;
using Domain.Posts;

namespace Application.Posts.AddNewPost
{
    public class AddNewPostService : IAddNewPostService
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public AddNewPostService(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public BaseDto<int> Execute(AddNewPostDto dto)
        {
            // Mapping the incoming DTO (Data Transfer Object) to the Post entity.
            var data = mapper.Map<Post>(dto);

            context.Posts.Add(data);
            context.SaveChanges();

            return new BaseDto<int>(data.Id, new List<string> { "Successfully registered" }, true);
        }
    }
}
