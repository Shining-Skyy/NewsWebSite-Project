using Application.Posts.PostServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.PostServices
{
    public interface IPostService
    {
        List<ListCategoryTypeDto> GetCategoryType();
    }
}
