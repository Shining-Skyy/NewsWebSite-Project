using System.Net;

namespace Application.Services.Sms
{
    public class SmsService
    {
        public Task Send(string PhoneNumber, string Token)
        {
            // Create a new instance of WebClient to send requests to the web service
            var client = new WebClient();

            // Construct the URL for the API request using string interpolation
            // The URL includes the phone number and token as query parameters
            string url = $"http://panel.kavenegar.com/v1/apikey/verify/lookup.json?receptor={PhoneNumber}&token={Token}&template=VerifyAccount";

            // Download the response from the specified URL as a string
            var content = client.DownloadString(url);
            
            return Task.CompletedTask;
        }
    }
}
