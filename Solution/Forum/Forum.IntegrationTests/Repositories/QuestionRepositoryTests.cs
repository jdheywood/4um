using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using Forum.IntegrationTests.Helpers;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Should;

namespace Forum.IntegrationTests.Repositories
{
    public class QuestionRepositoryTests
    {
        private readonly IQuestionRepository repository;
        private const int NumberOfQuestions = 5;

        public QuestionRepositoryTests()
        {
            var context = new Context();
            repository = context.Container.GetInstance<IQuestionRepository>();
        }

        [SetUp]
        public async void SetUp()
        {
            // Add some example questions
            var fixture = new Fixture();

            var questions = new List<Question>();
            fixture.RepeatCount = NumberOfQuestions;
            fixture.AddManyTo(questions);

            var identifier = 1;
            foreach (var question in questions)
            {
                var localQuestion = question;

                localQuestion.Id = identifier.ToString(CultureInfo.InvariantCulture);
                localQuestion.UserIdAsked = identifier + 10;
                localQuestion.UserIdAnswered = identifier < 3 ? 100 : 200;
                localQuestion.Removed = identifier == 3;

                var text = string.Empty;
                switch (identifier)
                {
                    case 1:
                        text = "question one...";
                        break;
                    case 2:
                        text = "question two...";
                        break;
                    case 3:
                        text = "question three...";
                        break;
                    case 4:
                        text = "question four...";
                        break;
                    case 5:
                        text = "question five...";
                        break;
                }
                localQuestion.Text = text;

                identifier++;
                
                AsyncHelpers.RunSync(() => repository.Add(localQuestion));
            }
        }

        [TearDown]
        public void TearDown()
        {
            repository.ClearCollection();
        }

        [Test]
        public async void GetAll_WhenCalledReturnsAllQuestions()
        {
            // Arrange
            const int expectedNumber = NumberOfQuestions;

            // Act
            var actual = await repository.GetAll();

            // Assert
            Assert.AreEqual(expectedNumber, actual.Count);
        }

        [Test]
        public async void GetByUserIdAsked_WhenCalledReturnsQuestionsAsked()
        {
            const int expectedQuestionCount = 1;
            const string expectedQuestionId = "1";
            const int userIdAsked = 11;

            // Act
            var actual = await repository.GetByUserIdAsked(userIdAsked, false);

            actual.Count.ShouldEqual(expectedQuestionCount);
            
            var firstOrDefault = actual.FirstOrDefault();
            
            if (firstOrDefault != null) firstOrDefault.Id.ShouldEqual(expectedQuestionId);
        }

        [Test]
        public async void GetByUserIdAnswered_WhenCalledReturnsQuestionsAnswered()
        {
            const int expectedQuestionCount = 3;
            string[] expectedQuestionIds = {"1", "2", "3"};
            const int userIdAnswered = 200;

            var actual = await repository.GetByUserIdAnswered(userIdAnswered);

            actual.Count.ShouldEqual(expectedQuestionCount);

            var countByExpectedIds = actual.Select(x => expectedQuestionIds.Contains(x.Id) && x.UserIdAnswered == userIdAnswered).Count();
            countByExpectedIds.ShouldEqual(expectedQuestionCount);
        }

        [Test]
        public void GetByIdArray_ReturnsQuestionsByArrayOfIdentifiers()
        {
            string[] questionIds = { "3", "4", "5" };
            const int expectedQuestionCount = 2;

            var actual = repository.GetByIdArray(questionIds, true).Result;

            actual.Count.ShouldEqual(expectedQuestionCount);
            var countByExpectedIds = actual.Select(x => questionIds.Contains(x.Id) && !x.Removed).Count();
            countByExpectedIds.ShouldEqual(expectedQuestionCount);            
        }

        [Test]
        public void GetById_WhenCalledWithExistingIdReturnsQuestion()
        {
            const string questionId = "5";
            const string expectedText = "question five...";

            var actual = repository.GetById(questionId);

            actual.Result.Text.ShouldEqual(expectedText);
        }

        [Test]
        public void GetById_WhenCalledWithNonExistingIdDoesNotReturnQuestion()
        {
            const string questionId = "6";

            var actual = repository.GetById(questionId);

            actual.Result.ShouldBeNull();
        }

        [Test]
        public void GetByText_WhenTextMatchedReturnsQuestion()
        {
            const string text = "question three...";
            const string expectedId = "3";

            var actual = repository.GetByText(text);

            actual.Result.Id.ShouldEqual(expectedId);
        }

        [Test]
        public void Search_SearchingForValidTermReturnsResults()
        {
            const string term = "question";
            const int expectedCount = 4;

            var actual = repository.Search(term);

            actual.Result.Count.ShouldEqual(expectedCount);
        }

        public void Search_SearchingForAnsweredQuestionsReturnsOnlyAnsweredQuestions()
        { }

        public void Search_SearchingForNonRemovedQuestionsReturnsOnlyNonRemovedQuestions()
        { }

        public void Search_SearchingForAnsweredAndNonRemovedQuestionsReturnsOnlyAnsweredAndNonRemovedQuestions()
        { }

        public void GetNew_ReturnsNewQuestionsOnly()
        { }

        public void GetNewCount_ReturnsCountOfNewQuestions()
        { }

        public void GetRepliedApproved_ReturnsQuestionsRepliedOrApprovedOnly()
        { }

        public void GetRepliedApprovedCount_ReturnsExpectedCount()
        { }

        public void GetRemoved_ReturnsOnlyRemovedQuestions()
        { }

        public void GetRemovedCount_ReturnsExpectedCount()
        { }

        public void Add_WhenCalledNewQuestionAdded()
        { }

        public void ReplaceById_WhenCalledWithValidIdQuestionUpdatedAsExpected()
        { }

        public void RemoveById_WhenCalledWithValidIdQuestionIsRemoved()
        { }
        
        public void RemoveById_WhenCalledWithInvalidIdQuestionCountRemainsUnchanged()
        { }

        [Test]
        public void ClearCollection_WhenCalledCollectionIsEmptied()
        {
            const int expectedQuestionCount = 0;

            repository.ClearCollection();

            var actual = repository.GetAll().Result.Count();

            actual.ShouldEqual(expectedQuestionCount);
        }
    }
}
