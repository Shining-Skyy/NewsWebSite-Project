using Application.Interfaces.Contexts;
using Application.Visitors.VisitorOnline.Dtos;
using Domain.Visitors;
using MongoDB.Driver;

namespace Application.Visitors.VisitorOnline
{
    public class VisitorOnlineService : IVisitorOnlineService
    {
        private readonly IMongoDbContext<OnlineVisitor> mongoDbContext;
        private readonly IMongoCollection<OnlineVisitor> mongoCollection;
        public VisitorOnlineService(IMongoDbContext<OnlineVisitor> mongoDbContext)
        {
            this.mongoDbContext = mongoDbContext;
            mongoCollection = mongoDbContext.GetCollection();
        }


        public void ConnectUser(string ClientId)
        {
            var exist = mongoCollection.AsQueryable().FirstOrDefault(p => p.ClientId == ClientId);
            if (exist == null)
            {
                mongoCollection.InsertOneAsync(new OnlineVisitor
                {
                    ClientId = ClientId,
                    Time = DateTime.Now,
                });
            }
        }

        public void DisConnectUser(string ClientId)
        {
            mongoCollection.FindOneAndDelete(p => p.ClientId == ClientId);
        }

        public VisitorOnlineDto GetAll()
        {
            var AllOnlineUsers = mongoCollection.AsQueryable().OrderByDescending(p => p.Time).Take(5).Select(p => new UserValuesDto()
            {
                ClientId = p.ClientId,
                Time = p.Time,
            }).ToList();

            return new VisitorOnlineDto()
            {
                UserValues = AllOnlineUsers,
            };
        }

        public int GetCount()
        {
            return mongoCollection.AsQueryable().Count();
        }
    }
}
