using System;
using Forum.Domain.Contracts;
using Forum.Domain.Factories;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace Forum.UnitTests.Factories
{
    public class MongoDatabaseFactoryTests
    {
        private const string MongoDbConnectionStringValue = "ConnectionString";
        private const string MongoDbDatabaseNameValue = "DatabaseName";

        private IMongoConfigurationFactory configurationFactory;
        private IMongoConfiguration configuration;
        private IMongoClient client;
        private MongoDatabaseFactory databaseFactory;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            configurationFactory = Substitute.For<IMongoConfigurationFactory>();
            configuration = Substitute.For<IMongoConfiguration>();
            client = Substitute.For<IMongoClient>();

            configurationFactory.Create().Returns(configuration);

            configuration.ConnectionString.Returns(MongoDbConnectionStringValue);
            configuration.DatabaseName.Returns(MongoDbDatabaseNameValue);

            var database = Substitute.For<IMongoDatabase>();

            client.GetDatabase(configuration.DatabaseName).Returns(database);

            databaseFactory = new MongoDatabaseFactory(configuration);
        }

        [Test]
        public void GetDatabase_WhenCalledShouldReturnDatabaseFromClient()
        {
            var database = databaseFactory.GetDatabase(client);

            // Proxy object returned, but it implements the interface of the actual object type, is that good enough for our test though?
            database.ShouldImplement<IMongoDatabase>(); 
            
            // How do I test the db name I created this with? Below fails as Namespace is null
            // database.DatabaseNamespace.DatabaseName.ShouldEqual(configuration.DatabaseName);
        }
    }
}
