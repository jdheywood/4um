using System.Collections.Generic;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using Forum.IntegrationTests.Helpers;
using NUnit.Framework;
using Ploeh.AutoFixture;

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

            foreach (var question in questions)
            {
                var localQuestion = question;
                AsyncHelpers.RunSync(() => repository.Add(localQuestion));
            }
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

        [TearDown]
        public void TearDown()
        {
            repository.ClearCollection();   
        }
    }
}
