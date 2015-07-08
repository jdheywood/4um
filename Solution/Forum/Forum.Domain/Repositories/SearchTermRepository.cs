using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Domain.Context;
using Forum.Domain.Contracts;
using Forum.Domain.Entities;
using MongoDB.Driver;

namespace Forum.Domain.Repositories
{
    public class SearchTermRepository : ISearchTermRepository
    {
        private readonly MongoCollectionBase<SearchTerm> collection; 

        public SearchTermRepository(IMongoContext context)
        {
            collection = context.GetSearchTermCollection();
        }

        public async Task<List<SearchTerm>> GetAll()
        {
            var filter = Builders<SearchTerm>.Filter.SizeGt("Text", 0);

            var sort = GetSort();

            return await collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<SearchTerm> GetById(string id)
        {
            var filter = Builders<SearchTerm>.Filter.Eq("_id", id);

            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<SearchTerm> GetByText(string text)
        {
            var filter = Builders<SearchTerm>.Filter.Eq("Text", text);

            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async void Add(SearchTerm searchterm)
        {
            await collection.InsertOneAsync(searchterm);
        }

        public async Task<UpdateResult> Update(SearchTerm searchTerm)
        {
            var filter = Builders<SearchTerm>.Filter.Eq("Text", searchTerm.Text);
            var update = Builders<SearchTerm>.Update
                .Set("Views", searchTerm.Views);

            return await collection.UpdateOneAsync(filter, update);
        }

        public async void ReplaceById(SearchTerm searchTerm)
        {
            var filter = Builders<SearchTerm>.Filter.Eq("_id", searchTerm.Id);

            await collection.ReplaceOneAsync(filter, searchTerm);
        }

        public async void RemoveById(string id)
        {
            var filter = Builders<SearchTerm>.Filter.Eq("_id", id);

            await collection.DeleteOneAsync(filter);
        }

        public async void ClearCollection()
        {
            var filter = Builders<SearchTerm>.Filter.Exists(term => term.Id);

            await collection.DeleteManyAsync(filter);
        }

        #region Privates

        private static SortDefinition<SearchTerm> GetSort()
        {
            var sortBuilder = Builders<SearchTerm>.Sort;
            return sortBuilder.Descending(term => term.Views);
        }

        #endregion
    }
}
