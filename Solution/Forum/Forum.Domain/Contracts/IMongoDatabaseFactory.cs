using MongoDB.Driver;

namespace Forum.Domain.Contracts
{
    public interface IMongoDatabaseFactory
    {
        IMongoDatabase GetDatabase(IMongoConfiguration configuration, IMongoClient client);
    }
}
