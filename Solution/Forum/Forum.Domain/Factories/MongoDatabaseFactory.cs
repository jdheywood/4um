using Forum.Domain.Contracts;
using MongoDB.Driver;

namespace Forum.Domain.Factories
{
    public class MongoDatabaseFactory : IMongoDatabaseFactory
    {
        private readonly IMongoConfigurationFactory configurationFactory;
        private readonly IMongoConfiguration configuration;
        private readonly IMongoClient client;

        public MongoDatabaseFactory(IMongoConfigurationFactory configurationFactory)
        {
            this.configurationFactory = configurationFactory;
            
            configuration = configurationFactory.Create();

            client = new MongoClient(configuration.ConnectionString);
        }

        public IMongoDatabase GetDatabase()
        {
            return client.GetDatabase(configuration.DatabaseName);
        }
    }

}
