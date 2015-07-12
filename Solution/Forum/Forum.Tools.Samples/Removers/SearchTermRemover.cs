using Forum.Core.Helpers;
using Forum.Domain.Contracts;

namespace Forum.Tools.Samples.Removers
{
    public class SearchTermRemover
    {
        private readonly ISearchTermRepository repository;

        public SearchTermRemover(ISearchTermRepository searchTermRepository)
        {
            repository = searchTermRepository;
        }

        public void Remove()
        {
            AsyncHelpers.RunSync(() => repository.ClearCollection());
        }
    }
}
