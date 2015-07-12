using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;

namespace Forum.Api.Controllers
{
    public class SearchTermsController : ApiController
    {
        private ISearchTermRepository repository;

        public SearchTermsController(ISearchTermRepository searchTermRepository)
        {
            repository = searchTermRepository;
        }

        // GET api/searchterms
        public async Task<IEnumerable<SearchTerm>> Get()
        {
            return await repository.GetAll();
        }

    }
}
