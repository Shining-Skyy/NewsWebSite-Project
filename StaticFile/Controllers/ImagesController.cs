using Microsoft.AspNetCore.Mvc;
using StaticFile.Dtos;

namespace StaticFile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        public ImagesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Post()
        {
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine("Resources", "Images");

                // Combine the current directory with the folder name to get the full path
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (files != null)
                {
                    //upload
                    return Ok(UploadFile(files));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error");
                throw new Exception("upload image error", ex);
            }
        }

        private UploadDto UploadFile(IFormFileCollection files)
        {
            string newName = Guid.NewGuid().ToString();
            var date = DateTime.Now;
            string folder = $@"Resources\Images\{date.Year}\{date.Month}-{date.Day}\";

            // Combine the web root path with the folder to get the full upload path
            var uploadsRootFolder = Path.Combine(_environment.WebRootPath, folder);

            // Check if the directory exists; if not, create it
            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            List<string> address = new List<string>();

            // Iterate through each file in the collection
            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    string fileName = newName + file.FileName;
                    var filePath = Path.Combine(uploadsRootFolder, fileName);

                    // Create a file stream to save the uploaded file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    address.Add(folder + fileName);
                }
            }

            return new UploadDto()
            {
                FileNameAddress = address,
            };
        }
    }
}
