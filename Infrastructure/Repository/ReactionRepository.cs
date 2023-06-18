using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IReactionRepository : IRepository<Reaction>
    {

    }
    public class ReactionRepository : Repository<Reaction>, IReactionRepository
    {
        public ReactionRepository(ZaloDbContext context) : base(context)
        {
        }
    }
}
