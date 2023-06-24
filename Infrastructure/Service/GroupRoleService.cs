using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Infrastructure.Service
{
    public interface IGroupRoleService
    {

        Task<List<GroupRole>> GetAll();
        Task<bool> AddGroupRole(GroupRole groupRole);
        Task<bool> RemoveGroupRole(int id);
        Task<bool> UpdateGroupRole(GroupRole groupRole);

    }
    public class GroupRoleService : IGroupRoleService
    {
   
        private readonly IGroupRoleRepository groupRoleRepository;
        public GroupRoleService(IGroupRoleRepository groupRoleRepository) 
        {
            this.groupRoleRepository = groupRoleRepository;
        }


        public async Task<bool> AddGroupRole(GroupRole groupRole)
        {
            bool result = await groupRoleRepository.Add(groupRole);
            return result;
        }

        public async Task<List<GroupRole>> GetAll()
        {
            return await groupRoleRepository.GetAll().ToListAsync();
        }

        public async Task<bool> RemoveGroupRole(int id)
        {
            GroupRole groupRole = await groupRoleRepository.GetById(id);
            bool result = await groupRoleRepository.Delete(groupRole);
            return result;
        }   

        public async Task<bool> UpdateGroupRole(GroupRole groupRole)
        {
            bool result = await groupRoleRepository.Update(groupRole);
            return result;
        }
    }
}
