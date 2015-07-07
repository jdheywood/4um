﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Forum.Domain.Entities;
using MongoDB.Driver;

namespace Forum.Domain.Contracts
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetAll();

        Task<List<Question>> GetByUserIdAsked(int id, bool hideRemoved);

        Task<List<Question>> GetByUserIdAnswered(int id);

        Task<List<Question>> GetByIdArray(string[] questionIds, bool hideRemoved);

        Task<Question> GetById(string id);

        Task<Question> GetByText(string text);

        Task<List<Question>> Search(string searchTerm, bool answeredOnly, bool hideRemoved);

        Task<List<Question>> GetNew(int pageNumber, int pageSize);

        Task<long> GetNewCount();

        Task<List<Question>> GetRepliedApproved(int pageNumber, int pageSize);

        Task<long> GetRepliedApprovedCount();

        Task<List<Question>> GetRemoved(int pageNumber, int pageSize);

        Task<long> GetRemovedCount();

        Task Add(Question question);
        
        void ReplaceById(Question question);

        void RemoveById(string id);

        void ClearCollection();
    }
}
