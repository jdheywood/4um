using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Forum.Core.Helpers;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using Ploeh.AutoFixture;

namespace Forum.Tools.Samples.Loaders
{
    public class AnswerLoader
    {
        private const int NumberOfAnswers = 3;

        private readonly IAnswerRepository answerRepository;
        private readonly IQuestionRepository questionRepository;

        public AnswerLoader(IAnswerRepository answerRepository, IQuestionRepository questionRepository)
        {
            this.answerRepository = answerRepository;
            this.questionRepository = questionRepository;
        }

        public async void SetUpAnswers()
        {
            var fixture = new Fixture();

            var answers = new List<Answer>();
            fixture.RepeatCount = NumberOfAnswers;
            fixture.AddManyTo(answers);

            var questions = await questionRepository.GetRepliedApproved(1, 10);

            CustomiseAndAddSampleAnswers(answers, questions);
        }

        private void CustomiseAndAddSampleAnswers(IEnumerable<Answer> answers, IEnumerable<Question> questions)
        {
            var identifier = 1;
            const int userIdOne = 123;
            const int userIdTwo = 999;

            var enumerable = questions as Question[] ?? questions.ToArray();
            var questionId = enumerable.Length > 0 ? enumerable[0].Id : "0";

            foreach (var answer in answers)
            {
                var localAnswer = answer;
                localAnswer.Id = identifier.ToString(CultureInfo.InvariantCulture);
                localAnswer.QuestionId = questionId;
                localAnswer.DateTime = DateTime.Now.AddDays(-identifier);
                localAnswer.Views = (identifier * 10);

                switch (identifier)
                {
                    case 1:
                        localAnswer.UserId = userIdOne;
                        localAnswer.Public = true;
                        localAnswer.Removed = true;
                        localAnswer.Text = "answer one...";
                        break;
                    case 2:
                        localAnswer.UserId = userIdOne;
                        localAnswer.Public = true;
                        localAnswer.Removed = false;
                        localAnswer.Text = "answer two...";
                        break;
                    case 3:
                        localAnswer.UserId = userIdOne;
                        localAnswer.Public = false;
                        localAnswer.Removed = true;
                        localAnswer.Text = "answer three...";
                        break;
                    case 4:
                        localAnswer.UserId = userIdTwo;
                        localAnswer.Public = false;
                        localAnswer.Removed = false;
                        localAnswer.Text = "answer four...";
                        break;
                }

                identifier++;

                AsyncHelpers.RunSync(() => answerRepository.Add(localAnswer));
            }
        }
    }
}
