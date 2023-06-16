using Infrastructure.Entities;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Infrastructure.Service
{
    public interface IReactionService
    {
        Task<Reaction> GetReaction(int id);
        IQueryable<Reaction> GetAll();
        Task<bool> AddReaction(Reaction reaction);
        Task<bool> RemoveReaction(Reaction reaction);
        Task<bool> UpdateReaction(Reaction reaction);
    }
    public class ReactionService : IReactionService
    {
        private IReactionRepository _repo;
        public ReactionService(IReactionRepository repo)
        {
            this._repo = repo;
        }
        public async Task<bool> AddReaction(Reaction reaction)
        {
            return await _repo.Add(reaction);
        }

        public IQueryable<Reaction> GetAll()
        {
            return _repo.GetAll();
        }

        public async Task<Reaction> GetReaction(int id)
        {
            return  await _repo.GetById(id);
        }


        public async Task<bool> RemoveReaction(Reaction reaction)
        {
            return await _repo.Delete(reaction);
        }

        public async Task<bool> UpdateReaction(Reaction reaction)
        {
            return await _repo.Update(reaction);    
        }
    }
}
