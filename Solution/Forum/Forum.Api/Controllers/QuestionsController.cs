using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        // GET api/questions/5
        public string Get(int id)
        {
            return "question";
        }

        // POST api/questions
        public void Post([FromBody]string value)
        {
        }

        // PUT api/questions/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/questions/5
        public void Delete(int id)
        {
        }
    }
}
