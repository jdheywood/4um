using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Domain.Entities;
using MongoDB.Driver;

namespace Forum.Domain.Contracts
{
    public interface IAnswerRepository
    {
        Task<List<Answer>> GetAll();

        Task<List<Answer>> GetByUserId(int id);
        
        Task<List<Answer>> GetNRecent(int maxResults);
        
        Task<Answer> GetById(string id);
        
        Task<Answer> GetByTextAndQuestionId(string text, string questionId);
        
        Task<Answer> GetByQuestionId(string questionId);
        
        void Add(Answer answer);
        
        Task<UpdateResult> Update(Answer answer);
        
        void ReplaceById(Answer answer);
        
        void RemoveById(string id);
        
        void ClearCollection();
    }
}