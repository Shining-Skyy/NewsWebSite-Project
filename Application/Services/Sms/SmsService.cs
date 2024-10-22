using System.Net;

namespace Application.Services.Sms
{
    public class SmsService
    {
        public Task Send(string PhoneNumber, string Token)
        {
            var client = new WebClient();
            string url = $"http://panel.kavenegar.com/v1/apikey/verify/lookup.json?receptor={PhoneNumber}&token={Token}&template=VerifyAccount";
            var content = client.DownloadString(url);
            return Task.CompletedTask;
        }
    }
}
