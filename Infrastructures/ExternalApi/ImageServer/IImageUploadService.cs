using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.ExternalApi.ImageServer
{
    public interface IImageUploadService
    {
        List<string> Upload(List<IFormFile> files);
    }
}
