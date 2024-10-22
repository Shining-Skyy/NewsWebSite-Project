using Application.Visitors.VisitorOnline.Dtos;

namespace Application.Visitors.VisitorOnline
{
    public interface IVisitorOnlineService
    {
        void ConnectUser(string ClientId);
        void DisConnectUser(string ClientId);
        int GetCount();
        VisitorOnlineDto GetAll();
    }
}
