using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Forum.Core.Helpers;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Should;

namespace Forum.IntegrationTests.Repositories
{
    public class AnswerRepositoryTests
    {
        private readonly IAnswerRepository repository;
        private const int NumberOfAnswers = 4;
        private const int UserIdOne = 123;
        private const int UserIdTwo = 999;

        public AnswerRepositoryTests()
        {
            var context = new Context();
            repository = context.Container.GetInstance<IAnswerRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();

            var answers = new List<Answer>();
            fixture.RepeatCount = NumberOfAnswers;
            fixture.AddManyTo(answers);

            var question = fixture.Create<Question>();

            CustomiseAndAddSampleAnswers(answers, question);
        }

        [TearDown]
        public async void TearDown()
        {
            await repository.ClearCollection();
        }


        [Test]
        public async void GetAll_WhenCalledReturnsAllAnswers()
        {
            // Arrange
            const int expectedNumber = NumberOfAnswers;

            // Act
            var actual = await repository.GetAll();

            // Assert
            Assert.AreEqual(expectedNumber, actual.Count);
        }

        [Test]
        public async void GetByUserId_WhenCalledReturnsAnswersForUser()
        {
            const int expectedAnswerCount = 3;
            string[] expectedAnswerIds = { "1", "2", "3" };
            
            var actual = await repository.GetByUserId(UserIdOne);

            actual.Count.ShouldEqual(expectedAnswerCount);
            var countByExpectedIds = actual.Select(x => expectedAnswerIds.Contains(x.Id) && x.UserId == UserIdOne).Count();
            countByExpectedIds.ShouldEqual(expectedAnswerCount);
        }

        [Test]
        public async void GetNRecent_ReturnsLessThanRequestedNumberOrderedByDateDescendingDueToPublicAndRemovedFlags()
        {
            const int expectedCount = 1;
            string[] expectedAnswerIds = { "1" };

            var actual = await repository.GetNRecent(NumberOfAnswers);

            var countByExpectedIds = actual.Select(x => expectedAnswerIds.Contains(x.Id) && x.UserId == UserIdOne).Count();
            countByExpectedIds.ShouldEqual(expectedCount);            
        }

        [Test]
        public async void GetById_WhenCalledWithValidIdentifierReturnsAnswer()
        {
            const string answerId = "2";
            const string expectedText = "answer two...";

            var actual = await repository.GetById(answerId);
            actual.Text.ShouldEqual(expectedText);
        }

        [Test]
        public async void GetById_WhenCalledWithInvalidIdentifierReturnsNull()
        {
            const string answerId = "999";

            var actual = await repository.GetById(answerId);
            actual.ShouldBeNull();
        }

        [Test]
        public async void GetByTextAndQuestionId_WhenMatchingAnswerInCollectionItIsReturned()
        {
            const string questionId = "1";
            const string text = "answer three...";

            var actual = await repository.GetByTextAndQuestionId(text, questionId);
            actual.Text.ShouldEqual(text);
            actual.QuestionId.ShouldEqual(questionId);
        }

        public async void GetByQuestionId_WhenMatchingAnswerInCollectionItIsReturned()
        { }

        public async void Add_RemoveById_CanAddNewAnswerThenRemoveByIdentifier()
        { }

        public async void Update_CanUpdateExistingAnswerById()
        { }

        public async void ReplaceById_CanReplaceExistingAnswer()
        { }

        public async void ClearCollection_WhenCalledCollectionIsEmptied()
        { }


        #region Privates

        private void CustomiseAndAddSampleAnswers(IEnumerable<Answer> answers, Question question)
        {
            var identifier = 1;

            question.Id = identifier.ToString(CultureInfo.InvariantCulture);

            foreach (var answer in answers)
            {
                var localAnswer = answer;
                localAnswer.Id = identifier.ToString(CultureInfo.InvariantCulture);
                localAnswer.QuestionId = question.Id;
                localAnswer.DateTime = DateTime.Now.AddDays(-identifier);
                localAnswer.Views = (identifier*10);

                switch (identifier)
                {
                    case 1:
                        localAnswer.UserId = UserIdOne;
                        localAnswer.Public = true;
                        localAnswer.Removed = true;
                        localAnswer.Text = "answer one...";
                        break;
                    case 2:
                        localAnswer.UserId = UserIdOne;
                        localAnswer.Public = true;
                        localAnswer.Removed = false;
                        localAnswer.Text = "answer two...";
                        break;
                    case 3:
                        localAnswer.UserId = UserIdOne;
                        localAnswer.Public = false;
                        localAnswer.Removed = true;
                        localAnswer.Text = "answer three...";
                        break;
                    case 4:
                        localAnswer.UserId = UserIdTwo;
                        localAnswer.Public = false;
                        localAnswer.Removed = false;
                        localAnswer.Text = "answer four...";
                        break;
                }

                identifier++;

                AsyncHelpers.RunSync(() => repository.Add(localAnswer));
            }
        }

        #endregion
    }
}
