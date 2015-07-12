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
            const int expectedNumber = NumberOfAnswers;

            var actual = await repository.GetAll();

            actual.Count.ShouldEqual(expectedNumber);
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

        [Test]
        public async void GetByQuestionId_WhenMatchingAnswerInCollectionItIsReturned()
        {
            const string questionId = "1";

            var actual = await repository.GetByQuestionId(questionId);

            actual.ShouldNotBeNull();
            actual.QuestionId.ShouldEqual(questionId);
        }

        [Test]
        public async void Add_RemoveById_CanAddNewAnswerThenRemoveByIdentifier()
        {
            var fixture = new Fixture();
            var newAnswer = fixture.Create<Answer>();
            
            const string newId = "987";
            newAnswer.Id = newId;

            const string newText = "I'm a new answer";
            newAnswer.Text = newText;

            await repository.Add(newAnswer);

            var actual = await repository.GetById(newId);
            
            actual.ShouldNotBeNull();
            actual.Text.ShouldEqual(newText);

            await repository.RemoveById(newId);

            var reCheck = await repository.GetById(newId);
            reCheck.ShouldBeNull();
        }

        [Test]
        public async void Update_CanUpdateExistingAnswerById()
        {
            const string id = "4";
            const string newText = "updated text";
            const int newViews = 99999;

            var before = await repository.GetById(id);

            before.ShouldNotBeNull();

            before.Text = newText;
            before.Views = newViews;

            var updateResult = await repository.Update(before);

            updateResult.ShouldNotBeNull();
            updateResult.ModifiedCount.ShouldEqual(1);

            var after = await repository.GetById(id);

            after.Text.ShouldEqual(newText);
            after.Views.ShouldEqual(newViews);
        }

        [Test]
        public async void ReplaceById_CanReplaceExistingAnswer()
        {
            const string idToReplace = "3";

            var fixture = new Fixture();
            var replacement = fixture.Create<Answer>();
            replacement.Id = idToReplace;

            var before = await repository.GetById(idToReplace);

            before.ShouldNotBeNull();

            await repository.ReplaceById(replacement);

            var after = await repository.GetById(idToReplace);

            after.ShouldNotBeNull();
            after.Text.ShouldEqual(replacement.Text);
            after.Views.ShouldEqual(replacement.Views);
            after.DateTime.ShouldNotEqual(before.DateTime);
            after.QuestionId.ShouldNotEqual(before.QuestionId);
        }

        [Test]
        public async void ClearCollection_WhenCalledCollectionIsEmptied()
        {
            const int expectedAnswerCount = 0;

            await repository.ClearCollection();

            var actual = await repository.GetAll();

            actual.Count.ShouldEqual(expectedAnswerCount);
        }


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
