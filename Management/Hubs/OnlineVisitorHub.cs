using Application.Visitors.VisitorOnline;
using Microsoft.AspNetCore.SignalR;

namespace Management.Hubs
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
            string visitorId = Context.GetHttpContext().Request.Cookies["VisitorId"];
            service.ConnectUser(visitorId);
            var count = service.GetCount();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string visitorId = Context.GetHttpContext().Request.Cookies["VisitorId"];
            service.DisConnectUser(visitorId);
            var count = service.GetCount();
            return base.OnDisconnectedAsync(exception);
        }
    }
}
