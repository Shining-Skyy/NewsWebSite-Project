using Application.Comments.Dto;
using Application.Dtos;
using Application.Interfaces.Contexts;
using AutoMapper;
using Common;
using Domain.Comments;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public bool ChangeStatus(int commentId)
        {
            // Check if the provided commentId is greater than 0
            if (commentId > 0)
            {
                // Retrieve the comment from the database that matches the given commentId
                var comment = context.Comments.FirstOrDefault(c => c.Id == commentId);
                if (comment != null)
                {
                    comment.IsActive = !comment.IsActive; // Change the status to the opposite of its current value
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public PaginatedItemsDto<CommentListDto> GetList(int pageIndex, int pageSize)
        {
            int rowsCount = 0;

            // Retrieve a paginated result of comments from the database
            var data = context.Comments.PagedResult(pageIndex, pageSize, out rowsCount);

            // Map the retrieved data to a list of CommentListDto objects
            var result = mapper.ProjectTo<CommentListDto>(data).ToList();

            return new PaginatedItemsDto<CommentListDto>(pageIndex, pageSize, rowsCount, result);
        }

        public List<CommentWithUserDto> GetListWhithId(int postId)
        {
            // Retrieve comments for the specified post ID
            var comments = context.Comments
                .Where(c => c.PostId == postId)
                .Where(c => c.IsActive == true)
                .ToList();

            // Get distinct user IDs from the retrieved comments
            var userIds = comments
                .Select(c => c.UserId)
                .Distinct()
                .ToList();

            // Fetch users from the identity database context whose IDs are in the userIds list
            var users = identityDatabaseContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToList();

            // Map comments to DTOs and include user information
            var commentWithUserDtos = comments.Select(comment =>
            {
                // Map the comment to CommentListDto
                var commentDto = mapper.Map<CommentListDto>(comment);

                // Map sub-comments to their DTOs if they exist
                commentDto.SubType = comment.SubType?.Select(subComment => new CommentListDto
                {
                    Id = subComment.Id,
                    Content = subComment.Content,
                    UserName = users.FirstOrDefault(u => u.Id == subComment.UserId)?.FullName ?? "Null",
                    ParentId = subComment.ParentTypeId,
                })
                .ToList() ?? new List<CommentListDto>();

                // Return a new CommentWithUserDto containing the comment and user information
                return new CommentWithUserDto
                {
                    Comment = commentDto,
                    User = users.FirstOrDefault(u => u.Id == comment.UserId)?.FullName ?? "Null"
                };
            }).ToList();

            return commentWithUserDtos;
        }

        public BaseDto Remove(int Id)
        {
            var data = context.Comments.Find(Id);

            context.Comments.Remove(data);
            context.SaveChanges();

            return new BaseDto(new List<string> { $"Comment removed successfully" }, true);
        }
    }
}
