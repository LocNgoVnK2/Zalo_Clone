using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IFriendListRepository : IRepository<FriendList>
    {

    }
    public class FriendListRepository : Repository<FriendList>, IFriendListRepository
    {
        public FriendListRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
