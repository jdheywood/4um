using Forum.Core.Configuration;
using Forum.Core.Contracts;
using Forum.Core.Factories;
using Forum.Core.Http;
using Forum.Core.Mapping;
using Forum.Domain.Configuration;
using Forum.Domain.Context;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using Forum.Domain.Factories;
using Forum.Domain.Repositories;
using SimpleInjector;

namespace Forum.Tools.Samples
{
    public static class SamplesContainer
    {
        public static Container RegisterDependencies(Container container)
        {
            container.Register<IMongoContext, MongoContext>();
            container.Register<IMongoClientFactory, MongoClientFactory>();
            container.Register<IMongoConfiguration, MongoConfiguration>();
            container.Register<IMongoConfigurationFactory, MongoConfigurationFactory>();
            container.Register<IMongoDatabaseFactory, MongoDatabaseFactory>();
            container.Register<IMongoCollectionFactory<Answer>, MongoCollectionFactory<Answer>>();
            container.Register<IMongoCollectionFactory<Question>, MongoCollectionFactory<Question>>();
            container.Register<IMongoCollectionFactory<SearchTerm>, MongoCollectionFactory<SearchTerm>>();
            container.Register<IAnswerRepository, AnswerRepository>();
            container.Register<IQuestionRepository, QuestionRepository>();
            container.Register<ISearchTermRepository, SearchTermRepository>();
            container.Register<IConfigurationRepository, ConfigurationRepository>();
            
            // TODO find a better way to register the above

            return container;
        }
    }
}