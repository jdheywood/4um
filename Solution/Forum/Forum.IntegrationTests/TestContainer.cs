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
using MongoDB.Driver;
using SimpleInjector;

namespace Forum.IntegrationTests
{
    public static class TestContainer
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
            container.Register<IHttpClient, HttpClient>();
            container.Register<IConfigurationRepository, ConfigurationRepository>();
            container.Register<IMapper, Mapper>();
            container.Register<IHttpActionResultFactory, HttpActionResultFactory>();

            // TODO find a better way to register the above

            //ConfigureAutoMapper(container);

            return container;
        }

        //private static void ConfigureAutoMapper(Container container)
        //{
        //    container.RegisterAll<Profile>(new BitbucketToDomainMappingProfile(), new DomainToDtoMappingProfile());

        //    Mapper.Initialize(x =>
        //    {
        //        var profiles = container.GetAllInstances<Profile>();

        //        foreach (var profile in profiles)
        //        {
        //            x.AddProfile(profile);
        //        }
        //    });

        //    Mapper.AssertConfigurationIsValid();
        //}
    }
}