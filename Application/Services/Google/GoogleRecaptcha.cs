using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Application.Services.Google
{
    public class GoogleRecaptcha
    {
        private readonly IConfiguration _configuration;
        public GoogleRecaptcha(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> Verify(string googleResponse)
        {
            // Retrieve the secret key from the configuration settings for Google reCAPTCHA
            string secret = _configuration["GoogleRecaptcha:Secretkey"];

            // Create an instance of HttpClient to send HTTP requests
            HttpClient httpClient = new HttpClient();

            // Send a POST request to the Google reCAPTCHA verification endpoint
            var result = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={googleResponse}", null);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            // Read the content of the response as a string
            string content = await result.Content.ReadAsStringAsync();

            // Parse the JSON response to access the success property
            dynamic jsonData = JObject.Parse(content);
            if (jsonData.success == "true")
            {
                return true;
            }

            return false;
        }
    }
}
