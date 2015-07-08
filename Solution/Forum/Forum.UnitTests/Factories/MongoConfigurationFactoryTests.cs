using Forum.Core.Contracts;
using Forum.Domain.Factories;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace Forum.UnitTests.Factories
{
    public class MongoConfigurationFactoryTests
    {
        private const string MongoDbConnectionStringKey = "MongoDb.ConnectionString";
        private const string MongoDbDatabaseNameKey = "MongoDb.DatabaseName";
        private const string MongoDbAnswerCollectionNameKey = "MongoDb.CollectionName.Answers";
        private const string MongoDbQuestionCollectionNameKey = "MongoDb.CollectionName.Questions";
        private const string MongoDbSearchTermCollectionNameKey = "MongoDb.CollectionName.SearchTerms";

        private const string MongoDbConnectionStringValue = "ConnectionString";
        private const string MongoDbDatabaseNameValue = "DatabaseName";
        private const string MongoDbAnswerCollectionNameValue = "Answers";
        private const string MongoDbQuestionCollectionNameValue = "Questions";
        private const string MongoDbSearchTermCollectionNameValue = "SearchTerms";

        private IConfigurationRepository configurationRepository;
        private MongoConfigurationFactory mongoConfigurationFactory;

        [SetUp]
        public void SetUp()
        {
            configurationRepository = Substitute.For<IConfigurationRepository>();

            configurationRepository.GetSimpleSetting<string>(MongoDbConnectionStringKey).Returns(MongoDbConnectionStringValue);
            configurationRepository.GetSimpleSetting<string>(MongoDbDatabaseNameKey).Returns(MongoDbDatabaseNameValue);
            configurationRepository.GetSimpleSetting<string>(MongoDbAnswerCollectionNameKey).Returns(MongoDbAnswerCollectionNameValue);
            configurationRepository.GetSimpleSetting<string>(MongoDbQuestionCollectionNameKey).Returns(MongoDbQuestionCollectionNameValue);
            configurationRepository.GetSimpleSetting<string>(MongoDbSearchTermCollectionNameKey).Returns(MongoDbSearchTermCollectionNameValue);

            mongoConfigurationFactory = new MongoConfigurationFactory(configurationRepository);
        }

        [Test]
        public void Create_WhenMongoConfigurationCreatedShouldReturnSettingsFromConfigurationRepository()
        {
            var mongoConfiguration = mongoConfigurationFactory.Create();
            mongoConfiguration.ConnectionString.ShouldEqual(MongoDbConnectionStringValue);
            mongoConfiguration.DatabaseName.ShouldEqual(MongoDbDatabaseNameValue);
            mongoConfiguration.AnswerCollectionName.ShouldEqual(MongoDbAnswerCollectionNameValue);
            mongoConfiguration.QuestionCollectionName.ShouldEqual(MongoDbQuestionCollectionNameValue);
            mongoConfiguration.SearchTermCollectionName.ShouldEqual(MongoDbSearchTermCollectionNameValue);
        }
    }
}
