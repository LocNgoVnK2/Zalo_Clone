using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IUserDataRepository : IRepository<UserData>
    {

    }
    public class UserDataRepository : Repository<UserData>, IUserDataRepository
    {
        public UserDataRepository(ZaloDbContext context) : base(context)
        {
        }
    }
}
