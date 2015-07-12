using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;

namespace Forum.Api.Controllers
{
    public class QuestionsController : ApiController
    {
        private IQuestionRepository repository;

        public QuestionsController(IQuestionRepository questionRepository)
        {
            repository = questionRepository;
        }

        // GET api/questions
        public async Task<IEnumerable<Question>> Get()
        {
            //return new string[] { "value1", "value2" };

            return await repository.GetAll();
        }

    }
}
