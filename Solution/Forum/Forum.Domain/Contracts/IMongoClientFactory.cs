using MongoDB.Driver;

namespace Forum.Domain.Contracts
{
    public interface IMongoClientFactory
    {
        IMongoClient Create(IMongoConfiguration configuration);
    }
}
