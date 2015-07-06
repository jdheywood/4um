using Forum.Core.Contracts;
using Forum.Domain.Configuration;
using Forum.Domain.Contracts;

namespace Forum.Domain.Factories
{
    public class MongoConfigurationFactory : IMongoConfigurationFactory
    {
        private const string MongoDbConnectionStringKey = "MongoDb.ConnectionString";
        private const string MongoDbDatabaseNameKey = "MongoDb.DatabaseName";
        private const string MongoDbAnswerCollectionNameKey = "MongoDb.CollectionName.Answers";
        private const string MongoDbQuestionCollectionNameKey = "MongoDb.CollectionName.Questions";
        private const string MongoDbSearchTermCollectionNameKey = "MongoDb.CollectionName.SearchTerms";

        private readonly IConfigurationRepository configurationRepository;

        public MongoConfigurationFactory(IConfigurationRepository configurationRepository)
        {
            this.configurationRepository = configurationRepository;
        }

        public IMongoConfiguration Create()
        {
            return new MongoConfiguration()
            {
                ConnectionString = configurationRepository.GetSimpleSetting<string>(MongoDbConnectionStringKey),
                DatabaseName = configurationRepository.GetSimpleSetting<string>(MongoDbDatabaseNameKey),
                AnswerCollectionName = configurationRepository.GetSimpleSetting<string>(MongoDbAnswerCollectionNameKey),
                QuestionCollectionName = configurationRepository.GetSimpleSetting<string>(MongoDbQuestionCollectionNameKey),
                SearchTermCollectionName = configurationRepository.GetSimpleSetting<string>(MongoDbSearchTermCollectionNameKey)
            };
        }
    }
}
