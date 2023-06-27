using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IMuteUserRepository : IRepository<MuteUser>
    {

    }
    public class MuteUserRepository : Repository<MuteUser>, IMuteUserRepository
    {
        public MuteUserRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
