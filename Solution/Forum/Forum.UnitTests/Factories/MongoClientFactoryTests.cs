using Forum.Domain.Contracts;
using Forum.Domain.Factories;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace Forum.UnitTests.Factories
{
    public class MongoClientFactoryTests
    {
        private const string MongoDbConnectionStringValue = "mongodb://connection-string:123";

        private IMongoConfiguration configuration;
        private MongoClientFactory mongoClientFactory;
        
        [SetUp]
        public void SetUp()
        {
            configuration = Substitute.For<IMongoConfiguration>();

            configuration.ConnectionString.Returns(MongoDbConnectionStringValue);
         
            mongoClientFactory = new MongoClientFactory();
        }

        [Test]
        public void Create_WhenMongoClientCreatedShouldReturnConnectionStringFromConfigurationRepository()
        {
            var mongoClient = mongoClientFactory.Create(configuration);

            mongoClient.ShouldBeType<MongoClient>();
        }
    }
}
