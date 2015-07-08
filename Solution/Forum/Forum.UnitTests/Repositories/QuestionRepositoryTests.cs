using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using Forum.Domain.Repositories;
using MongoDB.Driver;
using NUnit.Framework;
using NSubstitute;
using Ploeh.AutoFixture;

namespace Forum.UnitTests.Repositories
{
    public class QuestionRepositoryTests
    {
        private IMongoConfigurationFactory configurationFactory;
        private IMongoConfiguration configuration;
        private IMongoCollectionFactory<Question> collectionFactory;
        private MongoCollectionBase<Question> collection; 

        private QuestionRepository questionRepository;

        [SetUp]
        public void SetUp()
        {
            configurationFactory = Substitute.For<IMongoConfigurationFactory>();
            configuration = Substitute.For<IMongoConfiguration>();

            configurationFactory.Create().Returns(configuration);

            collectionFactory = Substitute.For<IMongoCollectionFactory<Question>>();
            collection = Substitute.For<MongoCollectionBase<Question>>();

            collectionFactory.GetCollection(configuration.QuestionCollectionName).Returns(collection);

            questionRepository = new QuestionRepository(configuration, collectionFactory);
        }

        [Ignore]
        [Test]
        public async void GetAll_WhenCalledReturnsAllDocumentsInCollection()
        {
            // Arrange
            var fixture = new Fixture();

            var filter = fixture.Create<FilterDefinition<Question>>();

            var allQuestionsCollection = new SubstituteFindFluentQuestion();
            var allQuestionsList = fixture.Create<List<Question>>();

            collection.Find(filter).Returns(allQuestionsCollection);
            allQuestionsCollection.ToListAsync().Returns(Task.FromResult(allQuestionsList));
            //collection.Find(filter).ToListAsync().Returns(Task.FromResult(allQuestionsList));

            // Act
            var actualResult = await questionRepository.GetAll();

            // Assert
            Assert.AreEqual(allQuestionsList, actualResult);
        }
    }

    public class SubstituteFindFluentQuestion : IFindFluent<Question, Question>
    {
        public Task<IAsyncCursor<Question>> ToCursorAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public IFindFluent<Question, Question> Limit(int? limit)
        {
            throw new NotImplementedException();
        }

        public IFindFluent<Question, TNewProjection> Project<TNewProjection>(ProjectionDefinition<Question, TNewProjection> projection)
        {
            throw new NotImplementedException();
        }

        public IFindFluent<Question, Question> Skip(int? skip)
        {
            throw new NotImplementedException();
        }

        public IFindFluent<Question, Question> Sort(SortDefinition<Question> sort)
        {
            throw new NotImplementedException();
        }

        public FilterDefinition<Question> Filter { get; set; }
        public FindOptions<Question, Question> Options { get; private set; }
    }
}
