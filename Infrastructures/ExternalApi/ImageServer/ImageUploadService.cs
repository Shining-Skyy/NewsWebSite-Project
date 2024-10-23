using Infrastructures.ExternalApi.ImageServer.Dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;

namespace Infrastructures.ExternalApi.ImageServer
{
    public class ImageUploadService : IImageUploadService
    {
        // This method uploads a list of image files and returns their addresses.
        public List<string> Upload(List<IFormFile> files)
        {
            // Create a new RestClient to send requests to the specified API endpoint.
            var client = new RestClient("https://localhost:44357/api/images");
            client.Timeout = -1;

            // Create a new POST request.
            var request = new RestRequest(Method.POST);

            // Iterate through each file in the provided list.
            foreach (var item in files)
            {
                byte[] bytes; // Declare a byte array to hold the file data.

                // Use a MemoryStream to read the file data.
                using (var ms = new MemoryStream())
                {
                    // Asynchronously copy the file's content to the MemoryStream.
                    item.CopyToAsync(ms);
                    // Convert the MemoryStream to a byte array.
                    bytes = ms.ToArray();
                }
                // Add the file to the request, specifying its name and content type.
                request.AddFile(item.FileName, bytes, item.FileName, item.ContentType);
            }
            // Execute the request and get the response from the server.
            IRestResponse response = client.Execute(request);

            // Deserialize the JSON response into an UploadDto object.
            UploadDto upload = JsonConvert.DeserializeObject<UploadDto>(response.Content);

            return upload.FileNameAddress;
        }
    }
}
