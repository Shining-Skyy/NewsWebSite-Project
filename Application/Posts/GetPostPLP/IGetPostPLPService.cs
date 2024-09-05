using Amazon.Runtime.Internal;
using Application.Dtos;
using Application.Posts.GetPostPLP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.GetPostPLP
{
    public interface IGetPostPLPService
    {
        PaginatedItemsDto<PostPLPDto> Execute(int page, int pageSize);
    }
}
