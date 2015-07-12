using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;

namespace Forum.Api.Controllers
{
    public class AnswersController : ApiController
    {
        private IAnswerRepository repository;

        public AnswersController(IAnswerRepository answerRepository)
        {
            repository = answerRepository;
        }

        // GET api/answers
        public async Task<IEnumerable<Answer>> Get()
        {
            return await repository.GetAll();
        }

    }
}
