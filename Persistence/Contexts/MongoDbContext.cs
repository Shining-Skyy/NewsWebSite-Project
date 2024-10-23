using Application.Interfaces.Contexts;
using MongoDB.Driver;

namespace Persistence.Contexts
{
    public class MongoDbContext<T> : IMongoDbContext<T>
    {
        private readonly IMongoDatabase db;
        private readonly IMongoCollection<T> mongoCollection;

        // Constructor for the MongoDbContext class.
        public MongoDbContext()
        {
            // Create a new MongoClient instance to connect to the MongoDB server.
            var client = new MongoClient();

            // Get the database named "VisitorDb" from the MongoDB server.
            db = client.GetDatabase("VisitorDb");

            // Get the collection corresponding to the type T, using the type's name as the collection name.
            mongoCollection = db.GetCollection<T>(typeof(T).Name);
        }

        // Method to retrieve the MongoDB collection for the specified type T.
        public IMongoCollection<T> GetCollection()
        {
            return mongoCollection;
        }
    }
}
