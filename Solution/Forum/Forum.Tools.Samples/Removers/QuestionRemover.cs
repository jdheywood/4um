using Forum.Core.Helpers;
using Forum.Domain.Contracts;

namespace Forum.Tools.Samples.Removers
{
    public class QuestionRemover
    {
        private readonly IQuestionRepository repository;

        public QuestionRemover(IQuestionRepository questionRepository)
        {
            repository = questionRepository;
        }

        public void Remove()
        {
            AsyncHelpers.RunSync(() => repository.ClearCollection());
        }
    }
}
