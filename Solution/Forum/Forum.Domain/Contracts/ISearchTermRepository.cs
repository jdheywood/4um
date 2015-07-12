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

        Task Add(SearchTerm bookmark);

        Task<UpdateResult> Update(SearchTerm searchTerm); // update Views by Term/Text

        Task ReplaceById(SearchTerm searchTerm);

        Task RemoveById(string id);

        Task ClearCollection();
    }
}
