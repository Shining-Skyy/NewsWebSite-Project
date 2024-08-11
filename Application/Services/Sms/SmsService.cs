using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
