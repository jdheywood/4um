using Forum.Core.Helpers;
using Forum.Domain.Contracts;

namespace Forum.Tools.Samples.Removers
{
    public class AnswerRemover
    {
        private IAnswerRepository repository;

        public AnswerRemover(IAnswerRepository answerRepository)
        {
            repository = answerRepository;
        }

        public void Remove()
        {
            AsyncHelpers.RunSync(() => repository.ClearCollection());
        }
    }
}
