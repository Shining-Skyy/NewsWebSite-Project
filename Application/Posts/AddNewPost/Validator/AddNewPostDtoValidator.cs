﻿using Application.Posts.AddNewPost.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.AddNewPost.Validator
{
    public class AddNewPostDtoValidator : AbstractValidator<AddNewPostDto>
    {
        public AddNewPostDtoValidator()
        {
            RuleFor(p => p.Titel).NotNull().Length(5, 60);
            RuleFor(p => p.PostDescription).NotNull().Length(10, 200);
            RuleFor(p => p.TimeRequired).NotNull();
            RuleFor(p => p.Content).NotNull();
        }
    }
}