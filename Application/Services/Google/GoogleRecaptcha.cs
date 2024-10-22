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
            string secret = _configuration["GoogleRecaptcha:Secretkey"];
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={googleResponse}", null);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            string content = await result.Content.ReadAsStringAsync();
            dynamic jsonData = JObject.Parse(content);
            if (jsonData.success == "true")
            {
                return true;
            }
            return false;
        }
    }
}
