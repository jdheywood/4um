using Forum.Domain.Contracts;
using MongoDB.Driver;

namespace Forum.Domain.Factories
{
    public class MongoClientFactory : IMongoClientFactory
    {
        public MongoClientFactory()
        { }

        public IMongoClient Create(IMongoConfiguration configuration)
        {
            return new MongoClient(configuration.ConnectionString);
        }
    }
}
