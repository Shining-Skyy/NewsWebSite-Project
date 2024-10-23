using Application.Posts.AddNewPost.Dto;
using FluentValidation;

namespace Application.Posts.AddNewPost.Validator
{
    public class AddNewPostDtoValidator : AbstractValidator<AddNewPostDto>
    {
        // Constructor for the AddNewPostDtoValidator class
        public AddNewPostDtoValidator()
        {
            RuleFor(p => p.Titel).NotNull().Length(5, 60);
            RuleFor(p => p.PostDescription).NotNull().Length(10, 200);
            RuleFor(p => p.TimeRequired).NotNull();
            RuleFor(p => p.Content).NotNull();
        }
    }
}
