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
    public class QuestionRepositoryTests
    {
        private readonly IQuestionRepository repository;
        private const int NumberOfQuestions = 7;

        public QuestionRepositoryTests()
        {
            var context = new Context();
            repository = context.Container.GetInstance<IQuestionRepository>();

            AsyncHelpers.RunSync(() => repository.ClearCollection());
        }

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();

            var questions = new List<Question>();
            fixture.RepeatCount = NumberOfQuestions;
            fixture.AddManyTo(questions);

            CustomiseAndAddSampleQuestions(questions);
        }

        [TearDown]
        public async void TearDown()
        {
            await repository.ClearCollection();
        }


        [Test]
        public async void GetAll_WhenCalledReturnsAllQuestions()
        {
            const int expectedNumber = NumberOfQuestions;

            var actual = await repository.GetAll();

            actual.Count.ShouldEqual(expectedNumber);
        }

        [Test]
        public async void GetByUserIdAsked_WhenCalledReturnsQuestionsAsked()
        {
            const int expectedQuestionCount = 1;
            const string expectedQuestionId = "1";
            const int userIdAsked = 11;

            var actual = await repository.GetByUserIdAsked(userIdAsked, false);

            actual.Count.ShouldEqual(expectedQuestionCount);
            var firstOrDefault = actual.FirstOrDefault();
            if (firstOrDefault != null) firstOrDefault.Id.ShouldEqual(expectedQuestionId);
        }

        [Test]
        public async void GetByUserIdAnswered_WhenCalledReturnsQuestionsAnswered()
        {
            const int expectedQuestionCount = 5;
            string[] expectedQuestionIds = {"1", "2", "3"};
            const int userIdAnswered = 200;

            var actual = await repository.GetByUserIdAnswered(userIdAnswered);

            actual.Count.ShouldEqual(expectedQuestionCount);
            var countByExpectedIds =
                actual.Select(x => expectedQuestionIds.Contains(x.Id) && x.UserIdAnswered == userIdAnswered).Count();
            countByExpectedIds.ShouldEqual(expectedQuestionCount);
        }

        [Test]
        public async void GetByIdArray_ReturnsQuestionsByArrayOfIdentifiers()
        {
            string[] questionIds = { "3", "4", "5" };
            const int expectedQuestionCount = 2;

            var actual = await repository.GetByIdArray(questionIds, true);

            actual.Count.ShouldEqual(expectedQuestionCount);
            var countByExpectedIds = actual.Select(x => questionIds.Contains(x.Id) && !x.Removed).Count();
            countByExpectedIds.ShouldEqual(expectedQuestionCount);            
        }

        [Test]
        public async void GetById_WhenCalledWithExistingIdReturnsQuestion()
        {
            const string questionId = "5";
            const string expectedText = "question five...";

            var actual = await repository.GetById(questionId);
            actual.Text.ShouldEqual(expectedText);
        }

        [Test]
        public async void GetById_WhenCalledWithNonExistingIdDoesNotReturnQuestion()
        {
            const string questionId = "8";

            var actual = await repository.GetById(questionId);
            actual.ShouldBeNull();
        }

        [Test]
        public async void GetByText_WhenTextMatchedReturnsQuestion()
        {
            const string text = "question three...";
            const string expectedId = "3";

            var actual = await repository.GetByText(text);
            actual.Id.ShouldEqual(expectedId);
        }

        [Test]
        public async void Search_SearchingForValidTermReturnsResults()
        {
            const string term = "question";
            const int expectedCount = 6;

            var actual = await repository.Search(term);
            actual.Count.ShouldEqual(expectedCount);
        }

        [Test]
        public async void Search_SearchingForAnsweredQuestionsReturnsOnlyAnsweredQuestions()
        {
            const string term = "question";
            const int expectedCount = 2;
            const int expectedQuestionAnswersCount = 1;
            const string expectedAnswerText = "the answer is...";

            var actual = await repository.Search(term, true, false);

            actual.Count.ShouldEqual(expectedCount);
            var firstOrDefault = actual.FirstOrDefault();
            if (firstOrDefault != null)
            {
                var answers = firstOrDefault.Answers;
                answers.Length.ShouldEqual(expectedQuestionAnswersCount);
                answers[0].CleanText.ShouldEqual(expectedAnswerText);
            }
        }

        [Test]
        public async void Search_SearchingForNonRemovedQuestionsReturnsOnlyNonRemovedQuestions()
        {
            const string term = "question";
            const int expectedCount = 6;

            var actual = await repository.Search(term);
            actual.Count.ShouldEqual(expectedCount);
        }

        [Test]
        public async void Search_SearchingForAnsweredAndNonRemovedQuestionsReturnsOnlyAnsweredAndNonRemovedQuestions()
        {
            const string term = "question";
            const int expectedCount = 1;

            var actual = await repository.Search(term, true);
            actual.Count.ShouldEqual(expectedCount);
        }

        [Test]
        public async void GetNew_ReturnsNewQuestionsOnly()
        {
            const int pageNumber = 1;
            const int pageSize = 10;
            const int expectedCount = 2;

            var actual = await repository.GetNew(pageNumber, pageSize);

            actual.Count.ShouldEqual(expectedCount);
            actual.Select(x => !x.Approved && !x.Archived && !x.Removed && !x.Replied).Count().ShouldEqual(expectedCount);
        }

        [Test]
        public async void GetNewCount_ReturnsCountOfNewQuestions()
        {
            const long expectedCount = 2;

            var actual = await repository.GetNewCount();
            actual.ShouldEqual(expectedCount);
        }

        [Test]
        public async void GetRepliedApproved_ReturnsQuestionsRepliedOrApprovedOnly()
        {
            const int pageNumber = 1;
            const int pageSize = 10;
            const int expectedCount = 3;

            var actual = await repository.GetRepliedApproved(pageNumber, pageSize);

            actual.Count.ShouldEqual(expectedCount);
            actual.Select(x => (x.Approved || x.Replied) && (!x.Archived && !x.Removed)).Count().ShouldEqual(expectedCount);
        }

        [Test]
        public async void GetRepliedApprovedCount_ReturnsExpectedCount()
        {
            const long expectedCount = 3;

            var actual = await repository.GetRepliedApprovedCount();

            actual.ShouldEqual(expectedCount);
        }

        [Test]
        public async void GetRemoved_ReturnsOnlyRemovedQuestions()
        {
            const int pageNumber = 1;
            const int pageSize = 10;
            const int expectedCount = 1;

            var actual = await repository.GetRemoved(pageNumber, pageSize);

            actual.Count.ShouldEqual(expectedCount);
            actual.Select(x => x.Removed).Count().ShouldEqual(expectedCount);
        }

        [Test]
        public async void GetRemovedCount_ReturnsExpectedCount()
        {
            const long expectedCount = 1;

            var actual = await repository.GetRemovedCount();

            actual.ShouldEqual(expectedCount);
        }

        [Test]
        public async void Add_RemoveById_CanAddNewQuestionThenRemoveByIdentifier()
        {
            var fixture = new Fixture();
            var question = fixture.Build<Question>().Create();
            const string newQuestionId = "99";
            question.Id = newQuestionId;

            await repository.Add(question);

            var questionFromRepo = await repository.GetById(newQuestionId);

            if (questionFromRepo == null) return;

            questionFromRepo.ShouldNotBeNull();
            questionFromRepo.Id.ShouldEqual(newQuestionId);

            AsyncHelpers.RunSync(() => repository.RemoveById(newQuestionId));
        }

        [Test]
        public async void ReplaceById_WhenCalledWithValidIdQuestionUpdatedAsExpected()
        {
            const string replacementQuestionId = "1";
            var questionFromRepo = await repository.GetById(replacementQuestionId);

            if (questionFromRepo == null) return;

            var fixture = new Fixture();
            var question = fixture.Build<Question>().Create();
            const string replacementQuestionText = "This is the replacement text...";
            question.Id = replacementQuestionId;
            question.Text = replacementQuestionText;

            await repository.ReplaceById(question);

            questionFromRepo = await repository.GetById(replacementQuestionId);

            questionFromRepo.ShouldNotBeNull();
            questionFromRepo.Text.ShouldEqual(replacementQuestionText);
        }
        
        [Test]
        public async void ClearCollection_WhenCalledCollectionIsEmptied()
        {
            const int expectedQuestionCount = 0;

            await repository.ClearCollection();

            var actual = await repository.GetAll();

            actual.Count.ShouldEqual(expectedQuestionCount);
        }


        #region privates

        private void CustomiseAndAddSampleQuestions(IEnumerable<Question> questions)
        {
            var identifier = 1;
            foreach (var question in questions)
            {
                var localQuestion = question;

                localQuestion.Id = identifier.ToString(CultureInfo.InvariantCulture);
                localQuestion.UserIdAsked = identifier + 10;
                localQuestion.UserIdAnswered = identifier < 3 ? 100 : 200;
                localQuestion.Text = GetSampleQuestionText(identifier);
                localQuestion.Answers = new QuestionAnswer[] { };

                switch (identifier)
                {
                    case 1:
                        SetSampleQuestionAsNew(localQuestion);
                        break;
                    case 2:
                        SetSampleQuestionAsApproved(localQuestion);
                        break;
                    case 3:
                        SetSampleQuestionAsNew(localQuestion);
                        localQuestion.Answers = new[] { GetSampleQuestionAnswer(identifier) };
                        break;
                    case 4:
                        SetSampleQuestionAsRemoved(localQuestion);
                        localQuestion.Answers = new[] { GetSampleQuestionAnswer(identifier) };
                        break;
                    case 5:
                        SetSampleQuestionAsReplied(localQuestion);
                        break;
                    case 6:
                        SetSampleQuestionAsArchived(localQuestion);
                        break;
                    case 7:
                        SetSampleQuestionAsReplied(localQuestion);
                        break;
                }

                identifier++;

                AsyncHelpers.RunSync(() => repository.Add(localQuestion));
            }
        }

        private static void SetSampleQuestionAsArchived(Question localQuestion)
        {
            localQuestion.Approved = false;
            localQuestion.Archived = true;
            localQuestion.Removed = false;
            localQuestion.Replied = false;
        }

        private static void SetSampleQuestionAsReplied(Question localQuestion)
        {
            localQuestion.Approved = false;
            localQuestion.Archived = false;
            localQuestion.Removed = false;
            localQuestion.Replied = true;
        }

        private static void SetSampleQuestionAsRemoved(Question localQuestion)
        {
            localQuestion.Removed = true;
        }

        private static void SetSampleQuestionAsApproved(Question localQuestion)
        {
            localQuestion.Approved = true;
            localQuestion.Archived = false;
            localQuestion.Removed = false;
            localQuestion.Replied = false;
        }

        private static void SetSampleQuestionAsNew(Question localQuestion)
        {
            localQuestion.Approved = false;
            localQuestion.Archived = false;
            localQuestion.Removed = false;
            localQuestion.Replied = false;
        }

        private static QuestionAnswer GetSampleQuestionAnswer(int identifier)
        {
            var questionAnswer = new QuestionAnswer
            {
                CleanText = "the answer is...",
                DateTime = DateTime.Now.ToString("d"),
                IsBookmarked = true,
                Public = true,
                Id = identifier.ToString(CultureInfo.InvariantCulture),
                Text = "the answer is..."
            };
            return questionAnswer;
        }

        private static string GetSampleQuestionText(int identifier)
        {
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
                case 6:
                    text = "question six...";
                    break;
                case 7:
                    text = "question seven...";
                    break;
            }
            return text;
        }

        #endregion
    }
}
