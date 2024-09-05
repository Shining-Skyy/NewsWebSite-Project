using Application.Posts.GetPostPDP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.GetPostPDP
{
    public interface IGetPostPDPService
    {
        PostPDPDto Execute(int Id);
    }
}
