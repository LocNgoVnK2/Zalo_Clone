using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IMuteGroupRepository : IRepository<MuteGroup>
    {

    }
    public class MuteGroupRepository : Repository<MuteGroup>, IMuteGroupRepository
    {
        public MuteGroupRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
