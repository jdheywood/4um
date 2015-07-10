using Forum.Domain.Entities;
using Forum.Domain.Factories;
using MongoDB.Driver;
using NSubstitute;
using NUnit.Framework;
using Should;

namespace Forum.UnitTests.Factories
{
    public class MongoCollectionFactoryTests
    {
        private const string QuestionCollectionName = "questions";
        private const string AnswerCollectionName = "answers";
        private const string SearchTermCollectionName = "searchterms";

        private MongoCollectionBase<Question> questionCollection;
        private MongoCollectionBase<Answer> answerCollection;
        private MongoCollectionBase<SearchTerm> searchTermCollection;

        private IMongoDatabase database;
        private MongoCollectionFactory<Question> mongoCollectionFactoryQuestion;
        private MongoCollectionFactory<Answer> mongoCollectionFactoryAnswer;
        private MongoCollectionFactory<SearchTerm> mongoCollectionFactorySearchTerm;
            
        [SetUp]
        public void SetUp()
        {
            database = Substitute.For<IMongoDatabase>();

            questionCollection = Substitute.For<MongoCollectionBase<Question>>();
            answerCollection = Substitute.For<MongoCollectionBase<Answer>>();
            searchTermCollection = Substitute.For<MongoCollectionBase<SearchTerm>>();

            database.GetCollection<Question>(QuestionCollectionName).Returns(questionCollection);
            database.GetCollection<Answer>(AnswerCollectionName).Returns(answerCollection);
            database.GetCollection<SearchTerm>(SearchTermCollectionName).Returns(searchTermCollection);
        }

        [Test]
        public void GetCollection_WhenCalledForQuestionsCollectionReturnsExpectedCollection()
        {
            mongoCollectionFactoryQuestion = new MongoCollectionFactory<Question>();

            var collection = mongoCollectionFactoryQuestion.GetCollection(database, QuestionCollectionName);

            collection.ShouldBeType(questionCollection.GetType());
            collection.ShouldBeSameAs(questionCollection);
        }

        [Test]
        public void GetCollection_WhenCalledForAnswersCollectionReturnsExpectedCollection()
        {
            mongoCollectionFactoryAnswer = new MongoCollectionFactory<Answer>();

            var collection = mongoCollectionFactoryAnswer.GetCollection(database, AnswerCollectionName);

            collection.ShouldBeType(answerCollection.GetType());
            collection.ShouldBeSameAs(answerCollection);
        }

        [Test]
        public void GetCollection_WhenCalledForSearchTermsCollectionReturnsExpectedCollection()
        {
            mongoCollectionFactorySearchTerm = new MongoCollectionFactory<SearchTerm>();

            var collection = mongoCollectionFactorySearchTerm.GetCollection(database, SearchTermCollectionName);

            collection.ShouldBeType(searchTermCollection.GetType());
            collection.ShouldBeSameAs(searchTermCollection);
        }
    }
}
