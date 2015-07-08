using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using MongoDB.Driver;

namespace Forum.Domain.Context
{
    public class MongoContext : IMongoContext
    {
        private IMongoConfigurationFactory configurationFactory;
        private IMongoClientFactory clientFactory;
        private IMongoDatabaseFactory databaseFactory;
        private readonly IMongoCollectionFactory<Question> questionCollectionFactory;
        private readonly IMongoCollectionFactory<Answer> answerCollectionFactory;
        private readonly IMongoCollectionFactory<SearchTerm> searchTermCollectionFactory;

        private readonly IMongoConfiguration configuration;
        private readonly IMongoDatabase database;

        public MongoContext(IMongoConfigurationFactory configurationFactory,
            IMongoClientFactory clientFactory,
            IMongoDatabaseFactory databaseFactory,
            IMongoCollectionFactory<Question> questionCollectionFactory,
            IMongoCollectionFactory<Answer> answerCollectionFactory,
            IMongoCollectionFactory<SearchTerm> searchTermCollectionFactory)
        {
            this.configurationFactory = configurationFactory;
            this.clientFactory = clientFactory;
            this.databaseFactory = databaseFactory;
            this.questionCollectionFactory = questionCollectionFactory;
            this.answerCollectionFactory = answerCollectionFactory;
            this.searchTermCollectionFactory = searchTermCollectionFactory;

            configuration = configurationFactory.Create();
            var client = clientFactory.Create(configuration);
            database = databaseFactory.GetDatabase(configuration, client);
        }

        public MongoCollectionBase<Question> GetQuestionCollection()
        {
            return questionCollectionFactory.GetCollection(database, configuration.QuestionCollectionName);
        }

        public MongoCollectionBase<Answer> GetAnswerCollection()
        {
            return answerCollectionFactory.GetCollection(database, configuration.AnswerCollectionName);
        }

        public MongoCollectionBase<SearchTerm> GetSearchTermCollection()
        {
            return searchTermCollectionFactory.GetCollection(database, configuration.SearchTermCollectionName);
        }
    }

    public interface IMongoContext
    {
        MongoCollectionBase<Question> GetQuestionCollection();

        MongoCollectionBase<Answer> GetAnswerCollection();
        
        MongoCollectionBase<SearchTerm> GetSearchTermCollection();
    }
}
