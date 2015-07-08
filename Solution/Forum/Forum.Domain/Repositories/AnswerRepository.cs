using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Domain.Context;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Forum.Domain.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly MongoCollectionBase<Answer> collection; 

        public AnswerRepository(IMongoContext context)
        {
            collection = context.GetAnswerCollection();
        }

        public async Task<List<Answer>> GetAll()
        {
            var filter = Builders<Answer>.Filter.Exists(answer => answer.Id);

            return await collection.Find(filter).ToListAsync();
        }

        public async Task<List<Answer>> GetByUserId(int id)
        {
            var filter = Builders<Answer>.Filter.Eq("UserId", id);

            return await collection.Find(filter).ToListAsync();
        }

        public async Task<List<Answer>> GetNRecent(int maxResults)
        {
            var filterBuilder = Builders<Answer>.Filter;
            var filter = filterBuilder.Eq(answer => answer.Public, true) & filterBuilder.Eq(answer => answer.Removed, false);

            var sortBuilder = Builders<Answer>.Sort;
            var sort = sortBuilder.Descending(answer => answer.DateTime);

            return await collection.Find(filter).Sort(sort).Limit(maxResults).ToListAsync();
        }

        public async Task<Answer> GetById(string id)
        {
            var filter = Builders<Answer>.Filter.Eq("_id", id);

            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Answer> GetByTextAndQuestionId(string text, string questionId)
        {
            var filterBuilder = Builders<Answer>.Filter;
            var filter = filterBuilder.Eq(answer => answer.Text, text) & filterBuilder.Eq(answer => answer.QuestionId, questionId);

            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Answer> GetByQuestionId(string questionId)
        {
            var filter = Builders<Answer>.Filter.Eq(answer => answer.QuestionId, questionId);

            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async void Add(Answer answer)
        {
            await collection.InsertOneAsync(answer);
        }

        public async Task<UpdateResult> Update(Answer answer)
        {
            var filter = Builders<Answer>.Filter.Eq("_id", answer.Id);
            var update = Builders<Answer>.Update
                .Set("QuestionId", answer.QuestionId)
                .Set("UserId", answer.UserId)
                .Set("DateTime", answer.DateTime)
                .Set("Text", answer.Text)
                .Set("CleanText", answer.CleanText)
                .Set("Tags", answer.Tags)
                .Set("Views", answer.Views)
                .Set("Public", answer.Public)
                .Set("Removed", answer.Removed);

            return await collection.UpdateOneAsync(filter, update);
        }

        public async void ReplaceById(Answer answer)
        {
            var filter = Builders<Answer>.Filter.Eq("_id", answer.Id);
            
            await collection.ReplaceOneAsync(filter, answer);
        }

        public async void RemoveById(string id)
        {
            var filter = Builders<Answer>.Filter.Eq("_id", id);

            await collection.DeleteOneAsync(filter);
        }

        public async void ClearCollection()
        {
            var filter = Builders<Answer>.Filter.Exists(answer => answer.Id);

            await collection.DeleteManyAsync(filter);
        }
    }
}
