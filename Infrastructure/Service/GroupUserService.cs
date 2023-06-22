using Infrastructure.Entities;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IGroupUserService
    {
        Task<List<GroupUser>> GetAllUserInGroup(string groupId);
        Task<bool> AddGroupUser(GroupUser groupUser);
        Task<bool> RemoveGroupUser(string idGroup, string idUser);
        Task<bool> UpdateGroupUser(GroupUser groupUser);
    }
    public class GroupUserService : IGroupUserService
    {
        IGroupUserRepository repository;
        public GroupUserService(IGroupUserRepository repository)
        {
            this.repository = repository;
        }
        public async Task<bool> AddGroupUser(GroupUser groupUser)
        {
            bool result = await repository.Add(groupUser);
            return result;
        }

        public async Task<List<GroupUser>> GetAllUserInGroup(string groupId)
        {
            return await repository.GetAll().Where(x=>x.IdGroup == groupId).Select(x=>x).ToListAsync();
        }

        public async Task<bool> RemoveGroupUser(string idGroup,string idUser)
        {
            GroupUser groupUser = await repository.GetAll().Where(x=>x.IdUser.Equals(idUser) && x.IdGroup.Equals(idGroup)).FirstOrDefaultAsync();
            bool result = await repository.Delete(groupUser);
            return result;
        }

        public async Task<bool> UpdateGroupUser(GroupUser groupUser)
        {
            bool result = await repository.Update(groupUser);
            return result;
        }
    }
}
