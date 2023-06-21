using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IFriendRequestRepository : IRepository<FriendRequest>
    {

    }
    public class FriendRequestRepository : Repository<FriendRequest>, IFriendRequestRepository
    {
        public FriendRequestRepository(ZaloDbContext context) : base(context)

        {
        }
    }
}
