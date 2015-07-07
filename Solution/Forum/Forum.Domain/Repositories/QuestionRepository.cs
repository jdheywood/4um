using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Forum.Domain.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly MongoCollectionBase<Question> collection;

        public QuestionRepository(IMongoConfigurationFactory configurationFactory, IMongoCollectionFactory<Question> collectionFactory)
        {
            var configuration = configurationFactory.Create();

            collection = collectionFactory.GetCollection(configuration.QuestionCollectionName);
        }

        public async Task<List<Question>> GetAll()
        {
            return await collection.Find(f => true).ToListAsync();
        }

        public async Task<List<Question>> GetByUserIdAsked(int id, bool hideRemoved)
        {
            var sort = GetSort();

            FilterDefinition<Question> filter;

            if (!hideRemoved)
            {
                filter = Builders<Question>.Filter.Eq("UserId", id);
            }
            else
            {
                var filterBuilder = Builders<Question>.Filter;
                filter = filterBuilder.Eq(question => question.UserIdAsked, id) & filterBuilder.Eq(question => question.Removed, false);

                return await collection.Find(filter).Sort(sort).ToListAsync();
            }

            return await collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<List<Question>> GetByUserIdAnswered(int id)
        {
            var filter = Builders<Question>.Filter.Eq("UserIdAnswered", id);

            var sort = GetSort();

            return await collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<List<Question>> GetByIdArray(string[] questionIds, bool hideRemoved)
        {
            FilterDefinition<Question> filter;

            if (!hideRemoved)
            {
                filter = Builders<Question>.Filter.In("_id", new BsonArray(questionIds));
            }
            else
            {
                var filterBuilder = Builders<Question>.Filter;
                filter = filterBuilder.In(question => question.Id, questionIds) & filterBuilder.Eq(question => question.Removed, false);
            }

            return await collection.Find(filter).ToListAsync();
        }

        public async Task<Question> GetById(string id)
        {
            var filter = Builders<Question>.Filter.Eq("_id", id);

            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Question> GetByText(string text)
        {
            var filter = Builders<Question>.Filter.Eq("Text", text);

            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Question>> Search(string searchTerm, bool answeredOnly, bool hideRemoved)
        {
            var filterBuilder = Builders<Question>.Filter;
            var filter = filterBuilder.Text(searchTerm);

            var searchResults = await collection.Find(filter).ToListAsync();

            if (answeredOnly && !hideRemoved)
            {
                return searchResults.Where(question => question.Answers.Length > 0).ToList();
            }
            if (!answeredOnly && hideRemoved)
            {
                return searchResults.Where(question => question.Removed).ToList();
            }

            return !answeredOnly 
                ? searchResults 
                : searchResults.Where(question => question.Answers.Length > 0 && question.Removed).ToList();
        }

        public async Task<List<Question>> GetNew(int pageNumber, int pageSize)
        {
            var filter = GetNewFilter();

            var sort = GetSort();

            return await collection.Find(filter).Sort(sort).Skip((pageNumber -1) * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task<long> GetNewCount()
        {
            var filter = GetNewFilter();

            return await collection.CountAsync(filter);
        }

        public async Task<List<Question>> GetAwaitingResponse(int pageNumber, int pageSize)
        {
            var filter = GetAwaitingResponseFilter();

            var sort = GetSort();

            return await collection.Find(filter).Sort(sort).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task<long> GetAwaitingResponseCount()
        {
            var filter = GetAwaitingResponseFilter();

            return await collection.CountAsync(filter);
        }

        public async Task<List<Question>> GetRepliedApproved(int pageNumber, int pageSize)
        {
            var filter = GetRepliedApprovedFilter();

            var sort = GetSort();

            return await collection.Find(filter).Sort(sort).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task<long> GetRepliedApprovedCount()
        {
            var filter = GetRepliedApprovedFilter();

            return await collection.CountAsync(filter);
        }

        public async Task<List<Question>> GetRemoved(int pageNumber, int pageSize)
        {
            var filter = GetRemovedFilter();

            var sort = GetSort();

            return await collection.Find(filter).Sort(sort).Skip((pageNumber - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task<long> GetRemovedCount()
        {
            var filter = GetRemovedFilter();

            return await collection.CountAsync(filter);
        }

        public async Task Add(Question question)
        {
            await collection.InsertOneAsync(question);
        }

        public async void ReplaceById(Question question)
        {
            var filter = Builders<Question>.Filter.Eq("_id", question.Id);

            await collection.ReplaceOneAsync(filter, question);
        }

        public async void RemoveById(string id)
        {
            var filter = Builders<Question>.Filter.Eq("_id", id);

            await collection.DeleteOneAsync(filter);
        }

        public async void ClearCollection()
        {
            var filter = Builders<Question>.Filter.Exists(question => question.Id);

            await collection.DeleteManyAsync(filter);
        }

        public static Task<List<Question>> ToListAsync(IFindFluent<Question, Question> findFluent)
        {
            return findFluent.ToListAsync();
        }

        #region Privates

        private static SortDefinition<Question> GetSort()
        {
            var sortBuilder = Builders<Question>.Sort;
            return sortBuilder.Descending(question => question.DateTime);
        }

        private static FilterDefinition<Question> GetNewFilter()
        {
            var filterBuilder = Builders<Question>.Filter;
            return filterBuilder.Eq(question => question.Approved, false)
                   & filterBuilder.Eq(question => question.Archived, false)
                   & filterBuilder.Eq(question => question.Removed, false)
                   & filterBuilder.Eq(question => question.Replied, false);
        }

        private static FilterDefinition<Question> GetAwaitingResponseFilter()
        {
            var filterBuilder = Builders<Question>.Filter;
            return filterBuilder.Eq(question => question.Archived, false)
                   & filterBuilder.Eq(question => question.Removed, false);
        }

        private static FilterDefinition<Question> GetRepliedApprovedFilter()
        {
            var filterBuilder = Builders<Question>.Filter;
            return (filterBuilder.Eq(question => question.Approved, true)
                    | filterBuilder.Eq(question => question.Replied, true)) &
                   (filterBuilder.Eq(question => question.Archived, false) &
                    filterBuilder.Eq(question => question.Removed, false));
        }

        private static FilterDefinition<Question> GetRemovedFilter()
        {
            var filterBuilder = Builders<Question>.Filter;
            return filterBuilder.Eq(question => question.Removed, true);
        }

        #endregion
    }
}
