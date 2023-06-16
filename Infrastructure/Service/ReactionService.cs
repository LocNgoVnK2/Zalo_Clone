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
        Reaction GetReaction(int id);
        IQueryable<Reaction> GetAll();
        void AddReaction(Reaction reaction);
        void RemoveReaction(Reaction reaction);
        void UpdateReaction(Reaction reaction);
    }
    public class ReactionService : IReactionService
    {
        private IReactionRepository _repo;
        public ReactionService(IReactionRepository repo)
        {
            this._repo = repo;
        }
        public void AddReaction(Reaction reaction)
        {
            _repo.Add(reaction);
        }

        public IQueryable<Reaction> GetAll()
        {
            return _repo.GetAll();
        }

        public Reaction GetReaction(int id)
        {
            return _repo.GetById(id);
        }


        public void RemoveReaction(Reaction reaction)
        {
            
        }

        public void UpdateReaction(Reaction reaction)
        {
            
        }
    }
}
