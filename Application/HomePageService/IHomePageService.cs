using Application.HomePageService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.HomePageService
{
    public interface IHomePageService
    {
        HomePageDto GetData();
    }
}
