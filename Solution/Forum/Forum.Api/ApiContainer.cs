using Forum.Core.Configuration;
using Forum.Core.Contracts;
using Forum.Core.Factories;
using Forum.Core.Http;
using Forum.Domain.Configuration;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using Forum.Domain.Factories;
using Forum.Domain.Repositories;
using SimpleInjector;

namespace Forum.Api
{
    public static class ApiContainer
    {
        public static Container RegisterDependencies(Container container)
        {
            // Register your types, for instance using the RegisterWebApiRequest extension from the integration package:
            container.RegisterWebApiRequest<IMongoConfiguration, MongoConfiguration>();
            container.RegisterWebApiRequest<IMongoConfigurationFactory, MongoConfigurationFactory>();
            container.RegisterWebApiRequest<IMongoDatabaseFactory, MongoDatabaseFactory>();
            container.RegisterWebApiRequest<IMongoCollectionFactory<Answer>, MongoCollectionFactory<Answer>>();
            container.RegisterWebApiRequest<IMongoCollectionFactory<Question>, MongoCollectionFactory<Question>>();
            container.RegisterWebApiRequest<IMongoCollectionFactory<SearchTerm>, MongoCollectionFactory<SearchTerm>>();
            container.RegisterWebApiRequest<IAnswerRepository, AnswerRepository>();
            container.RegisterWebApiRequest<IQuestionRepository, QuestionRepository>();
            container.RegisterWebApiRequest<ISearchTermRepository, SearchTermRepository>();
            container.RegisterWebApiRequest<IHttpClient, HttpClient>();
            container.RegisterWebApiRequest<IConfigurationRepository, ConfigurationRepository>();
            container.RegisterWebApiRequest<IMapper, Core.Mapping.Mapper>();
            container.RegisterWebApiRequest<IHttpActionResultFactory, HttpActionResultFactory>();

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