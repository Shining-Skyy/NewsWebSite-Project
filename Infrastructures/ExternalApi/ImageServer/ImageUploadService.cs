using Infrastructures.ExternalApi.ImageServer.Dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Infrastructures.ExternalApi.ImageServer
{
    public class ImageUploadService : IImageUploadService
    {
        public List<string> Upload(List<IFormFile> files)
        {
            var client = new RestClient("https://localhost:44357/api/images");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            foreach (var item in files)
            {
                byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    item.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }
                request.AddFile(item.FileName, bytes, item.FileName, item.ContentType);
            }
            IRestResponse response = client.Execute(request);
            UploadDto upload = JsonConvert.DeserializeObject<UploadDto>(response.Content);

            return upload.FileNameAddress;
        }
    }
}
