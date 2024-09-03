using Application.Dtos;
using Application.Posts.AddNewPost.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.AddNewPost
{
    public interface IAddNewPostService
    {
        BaseDto<int> Execute(AddNewPostDto dto);
    }
}
