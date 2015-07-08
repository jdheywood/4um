using Forum.Domain.Contracts;
using MongoDB.Driver;

namespace Forum.Domain.Factories
{
    public class MongoDatabaseFactory : IMongoDatabaseFactory
    {
        public MongoDatabaseFactory()
        { }

        public IMongoDatabase GetDatabase(IMongoConfiguration configuration, IMongoClient client)
        {
            return client.GetDatabase(configuration.DatabaseName);
        }
    }

}
