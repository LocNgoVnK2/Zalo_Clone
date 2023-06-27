using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IRoleRepository : IRepository<Role>
    {

    }
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ZaloDbContext context) : base(context)

        {
        }
    }
 
}
