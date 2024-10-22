using Application.Comments.Dto;
using Application.Dtos;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Comments;

namespace Application.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        private readonly IIdentityDatabaseContext identityDatabaseContext;
        public CommentsService(IDataBaseContext context, IMapper mapper, IIdentityDatabaseContext identityDatabaseContext)
        {
            this.context = context;
            this.mapper = mapper;
            this.identityDatabaseContext = identityDatabaseContext;
        }

        public BaseDto<CommentDto> Add(CommentDto commentDto)
        {
            var model = mapper.Map<Comment>(commentDto);
            context.Comments.Add(model);
            context.SaveChanges();
            return new BaseDto<CommentDto>(commentDto, new List<string> { $"Your comment has been registered." }, true);
        }

        public List<CommentWithUserDto> GetList(int postId)
        {
            var comments = context.Comments.Where(p => p.PostId == postId).ToList();
            var userIds = comments.Select(c => c.UserId).Distinct().ToList();
            var users = identityDatabaseContext.Users
                .Where(u => userIds.Contains(u.Id)).ToList();

            var commentWithUserDtos = comments.Select(comment =>
            {
                var commentDto = mapper.Map<CommentListDto>(comment);
                commentDto.SubType = comment.SubType?.Select(subComment => new CommentListDto
                {
                    Id = subComment.Id,
                    Content = subComment.Content,
                    UserName = users.FirstOrDefault(u => u.Id == subComment.UserId)?.FullName ?? "Null",
                    ParentId = subComment.ParentTypeId,
                }).ToList() ?? new List<CommentListDto>();

                return new CommentWithUserDto
                {
                    Comment = commentDto,
                    User = users.FirstOrDefault(u => u.Id == comment.UserId)?.FullName ?? "Null"
                };
            }).ToList();

            return commentWithUserDtos;
        }
    }
}
