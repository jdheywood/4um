using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Domain.Entities;
using MongoDB.Driver;

namespace Forum.Domain.Contracts
{
    public interface ISearchTermRepository
    {
        Task<List<SearchTerm>> GetAll();

        Task<SearchTerm> GetById(string id);

        Task<SearchTerm> GetByText(string text);

        void Add(SearchTerm bookmark);

        Task<UpdateResult> Update(SearchTerm searchTerm); // update Views by Term/Text

        void ReplaceById(SearchTerm searchTerm);

        void RemoveById(string id);

        void ClearCollection();
    }
}
