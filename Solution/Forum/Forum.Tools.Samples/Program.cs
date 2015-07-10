using System;
using System.Collections.Generic;
using System.Globalization;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using Ploeh.AutoFixture;
using SimpleInjector;

namespace Forum.Tools.Samples
{
    public class Program
    {
        public static Container Container { get; set; }
        public static IMongoContext MongoContext { get; set; }

        private static IQuestionRepository repository;

        public static int NumberOfQuestions { get; set; }


        public static void Main(string[] args)
        {
            SetUpcontainerAndContext();

            repository = Container.GetInstance<IQuestionRepository>();

            Console.WriteLine("hello world");

            SetUpQuestions();
        }


        #region Privates

        private static void SetUpcontainerAndContext()
        {
            Container = new Container();

            SamplesContainer.RegisterDependencies(Container);

            Container.Verify();

            MongoContext = Container.GetInstance<IMongoContext>();
        }

        private static void SetUpQuestions()
        {
            var fixture = new Fixture();

            var questions = new List<Question>();
            fixture.RepeatCount = NumberOfQuestions;
            fixture.AddManyTo(questions);

            CustomiseAndAddSampleQuestions(questions);
        }

        private static async void CustomiseAndAddSampleQuestions(IEnumerable<Question> questions)
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

                await repository.Add(localQuestion);
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
