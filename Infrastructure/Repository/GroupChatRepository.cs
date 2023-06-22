using Infrastructure.Data;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IGroupRoleRepository : IRepository<GroupRole>
    {

    }
    public class GroupRoleRepository : Repository<GroupRole>, IGroupRoleRepository
    {
        public GroupRoleRepository(ZaloDbContext context) : base(context)
        {
        }
    }
    public interface IGroupChatRepository : IRepository<GroupChat>
    {

    }
    public class GroupChatRepository: Repository<GroupChat>, IGroupChatRepository
    {
        public GroupChatRepository(ZaloDbContext context) : base (context)
        { 
        }
    }
    public interface IGroupUserRepository : IRepository<GroupUser>
    {

    }
    public class GroupUserRepository : Repository<GroupUser>, IGroupUserRepository
    {
        public GroupUserRepository(ZaloDbContext context) : base(context)
        {
        }
    }
}
