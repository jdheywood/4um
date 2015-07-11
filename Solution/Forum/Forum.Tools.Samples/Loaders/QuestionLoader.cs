using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Forum.Core.Helpers;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using Ploeh.AutoFixture;

namespace Forum.Tools.Samples.Loaders
{
    public class QuestionLoader
    {
        private const int NumberOfQuestions = 3;

        private readonly IQuestionRepository repository;

        public QuestionLoader(IQuestionRepository questionReository)
        {
            repository = questionReository;
        }

        public async Task SetUpQuestions()
        {
            var fixture = new Fixture();

            var questions = new List<Question>();
            fixture.RepeatCount = NumberOfQuestions;
            fixture.AddManyTo(questions);

            await CustomiseAndAddSampleQuestions(questions);
        }

        private async Task CustomiseAndAddSampleQuestions(IEnumerable<Question> questions)
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

        private void SetSampleQuestionAsArchived(Question localQuestion)
        {
            localQuestion.Approved = false;
            localQuestion.Archived = true;
            localQuestion.Removed = false;
            localQuestion.Replied = false;
        }

        private void SetSampleQuestionAsReplied(Question localQuestion)
        {
            localQuestion.Approved = false;
            localQuestion.Archived = false;
            localQuestion.Removed = false;
            localQuestion.Replied = true;
        }

        private void SetSampleQuestionAsRemoved(Question localQuestion)
        {
            localQuestion.Removed = true;
        }

        private void SetSampleQuestionAsApproved(Question localQuestion)
        {
            localQuestion.Approved = true;
            localQuestion.Archived = false;
            localQuestion.Removed = false;
            localQuestion.Replied = false;
        }

        private void SetSampleQuestionAsNew(Question localQuestion)
        {
            localQuestion.Approved = false;
            localQuestion.Archived = false;
            localQuestion.Removed = false;
            localQuestion.Replied = false;
        }

        private QuestionAnswer GetSampleQuestionAnswer(int identifier)
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

        private string GetSampleQuestionText(int identifier)
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
    }
}
