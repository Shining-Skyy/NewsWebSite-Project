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

        // This method connects a user by their ClientId.
        // If the user does not already exist in the collection, a new entry is created.
        public void ConnectUser(string ClientId)
        {
            // Check if a user with the given ClientId already exists in the collection.
            var exist = mongoCollection.AsQueryable().FirstOrDefault(p => p.ClientId == ClientId);

            // If the user does not exist, insert a new OnlineVisitor record.
            if (exist == null)
            {
                mongoCollection.InsertOneAsync(new OnlineVisitor
                {
                    ClientId = ClientId,
                    Time = DateTime.Now,
                });
            }
        }

        // This method disconnects a user by their ClientId.
        // It finds the user in the collection and deletes their record.
        public void DisConnectUser(string ClientId)
        {
            // Find the user by ClientId and delete their record from the collection.
            mongoCollection.FindOneAndDelete(p => p.ClientId == ClientId);
        }

        // This method retrieves all currently online users.
        // It returns the top 5 users based on their connection time, in descending order.
        public VisitorOnlineDto GetAll()
        {
            // Query the collection to get all online users, ordered by connection time.
            var AllOnlineUsers = mongoCollection
                .AsQueryable()
                .OrderByDescending(p => p.Time)
                .Take(5)
                .Select(p => new UserValuesDto()
                {
                    ClientId = p.ClientId,
                    Time = p.Time,
                })
                .ToList();

            return new VisitorOnlineDto()
            {
                UserValues = AllOnlineUsers,
            };
        }

        // This method returns the total count of online users in the collection.
        public int GetCount()
        {
            return mongoCollection.AsQueryable().Count();
        }
    }
}
