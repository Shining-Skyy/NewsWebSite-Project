using Microsoft.AspNetCore.Http;

namespace Infrastructures.ExternalApi.ImageServer
{
    public interface IImageUploadService
    {
        List<string> Upload(List<IFormFile> files);
    }
}
