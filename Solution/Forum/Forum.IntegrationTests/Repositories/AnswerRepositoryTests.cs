using System.Collections.Generic;
using Forum.Core.Helpers;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Forum.IntegrationTests.Repositories
{
    public class AnswerRepositoryTests
    {
        private readonly IAnswerRepository repository;
        private const int NumberOfAnswers = 3;

        public AnswerRepositoryTests()
        {
            var context = new Context();
            repository = context.Container.GetInstance<IAnswerRepository>();
        }

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();

            var answers = fixture.CreateMany<Answer>();

            CustomiseAndAddSampleAnswers(answers);
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

        public async void GetByUserId_WhenCalledReturnsAnswersForUser()
        { }

        public async void GetNRecent_ReturnsRequestedNumberOrderedByDateDescending()
        { }

        public async void GetById_WhenCalledWithValidIdentifierReturnsAnswer()
        { }

        public async void GetById_WhenCalledWithInvalidIdentifierReturnsNull()
        { }

        public async void GetByTextAndQuestionId_WhenMatchingAnswerInCollectionItIsReturned()
        { }

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

        private void CustomiseAndAddSampleAnswers(IEnumerable<Answer> answers)
        {
            var identifier = 1;
            foreach (var answer in answers)
            {
                var localAnswer = answer;

                AsyncHelpers.RunSync(() => repository.Add(localAnswer));
            }

        }

        #endregion
    }
}
