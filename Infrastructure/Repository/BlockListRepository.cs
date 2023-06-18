using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IBlockListRepository : IRepository<BlockList>
    {

    }
    public class BlockListRepository : Repository<BlockList>, IBlockListRepository
    {
        public BlockListRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
