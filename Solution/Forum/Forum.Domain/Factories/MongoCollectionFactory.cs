using Forum.Domain.Contracts;
using MongoDB.Driver;

namespace Forum.Domain.Factories
{
    public class MongoCollectionFactory<T> : IMongoCollectionFactory<T>
    {
        public MongoCollectionFactory()
        { }

        public MongoCollectionBase<T> GetCollection(IMongoDatabase database, string collectionName)
        {
            return (MongoCollectionBase<T>)database.GetCollection<T>(collectionName);
        }
    }
}
