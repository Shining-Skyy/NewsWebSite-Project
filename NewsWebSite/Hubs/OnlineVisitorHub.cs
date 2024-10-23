using Application.Visitors.VisitorOnline;
using Microsoft.AspNetCore.SignalR;

namespace NewsWebSite.Hubs
{
    public class OnlineVisitorHub : Hub
    {
        private readonly IVisitorOnlineService service;
        public OnlineVisitorHub(IVisitorOnlineService service)
        {
            this.service = service;
        }

        public override Task OnConnectedAsync()
        {
            // Retrieve the VisitorId from the HTTP request cookies
            string visitorId = Context.GetHttpContext().Request.Cookies["VisitorId"];

            // Call the service to connect the user using the retrieved VisitorId
            service.ConnectUser(visitorId);
            service.GetCount();

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // Retrieve the VisitorId from the HTTP request cookies again upon disconnection
            string visitorId = Context.GetHttpContext().Request.Cookies["VisitorId"];

            // Call the service to disconnect the user using the retrieved VisitorId
            service.DisConnectUser(visitorId);
            service.GetCount();

            return base.OnDisconnectedAsync(exception);
        }
    }
}
