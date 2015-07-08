using Forum.Domain.Entities;
using MongoDB.Driver;

namespace Forum.Domain.Contracts
{
    public interface IMongoContext
    {
        MongoCollectionBase<Question> GetQuestionCollection();

        MongoCollectionBase<Answer> GetAnswerCollection();

        MongoCollectionBase<SearchTerm> GetSearchTermCollection();

        void EnforceTextIndexes();
    }
}
