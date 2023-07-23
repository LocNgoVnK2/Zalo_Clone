using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IUserContactRepository : IRepository<UserContact>
    {

    }
    public class UserContactRepository : Repository<UserContact>, IUserContactRepository
    {
        public UserContactRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
