using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forum.Domain.Contracts;

namespace Forum.Domain.Configuration
{
    public class MongoConfiguration : IMongoConfiguration
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
        
        public string AnswerCollectionName { get; set; }
        
        public string QuestionCollectionName { get; set; }
        
        public string SearchTermCollectionName { get; set; }
    }
}
