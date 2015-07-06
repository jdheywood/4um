using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using Forum.Domain.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using NSubstitute;
using Ploeh.AutoFixture;

namespace Forum.UnitTests.Repositories
{
    public class AnswerRepositoryTests
    {
        private IMongoConfiguration configuration;
        private IMongoCollectionFactory<Answer> factory; 
        private IMongoCollection<Answer> collection; 

        private AnswerRepository answerRepository;

        [SetUp]
        public void SetUp()
        {
            configuration = Substitute.For<IMongoConfiguration>();
            factory = Substitute.For<IMongoCollectionFactory<Answer>>();
            collection = Substitute.For<IMongoCollection<Answer>>();

            answerRepository = new AnswerRepository(configuration, factory);
        }

        [Test]
        public async void GetAll_WhenCalledReturnsAllDocumentsInCollection()
        {
            // Arrange
            var fixture = new Fixture();

            var filter = fixture.Create<FilterDefinition<Answer>>();

            var allAnswersCollection = fixture.Create<IFindFluent<Answer, Answer>>();

            var allAnswersList = fixture.Create<Task<List<Answer>>>();

            collection.Find(filter).Returns(allAnswersCollection);
                
            allAnswersCollection.ToListAsync().Returns(allAnswersList);

            // Act
            var actualResult = await answerRepository.GetAll();

            // Assert
            Assert.AreEqual(allAnswersList, actualResult);
        }

    }
}
