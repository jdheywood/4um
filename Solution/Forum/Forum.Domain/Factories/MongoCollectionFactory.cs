using Forum.Domain.Contracts;
using MongoDB.Driver;

namespace Forum.Domain.Factories
{
    public class MongoCollectionFactory<T> : IMongoCollectionFactory<T>
    {
        private readonly IMongoDatabaseFactory databaseFactory;

        public MongoCollectionFactory(IMongoDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        public MongoCollectionBase<T> GetCollection(string collectionName)
        {
            var db = databaseFactory.GetDatabase();

            return (MongoCollectionBase<T>)db.GetCollection<T>(collectionName);
        }
    }
}
