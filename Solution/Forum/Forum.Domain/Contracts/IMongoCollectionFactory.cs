using MongoDB.Driver;

namespace Forum.Domain.Contracts
{
    public interface IMongoCollectionFactory<T>
    {
        MongoCollectionBase<T> GetCollection(string collectionName);
    }
}
