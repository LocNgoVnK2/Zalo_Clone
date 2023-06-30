using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IRoleService
    {
        Task<Role> GetRole(string id);
        IQueryable<Role> GetAllRoles();
        Task<bool> AddRole(string name);
        Task<bool> RemoveRole(Role role);
        Task<bool> UpdateRole(Role role);
    }
    public class RoleService:IRoleService
    {

        
        private readonly IRoleRepository _roleRepository;
        
        public RoleService( IRoleRepository roleRepository)
        {
       
            _roleRepository = roleRepository;
        }

        public async Task<Role> GetRole(string id)
        {
            var result = await _roleRepository.GetById(id);
            return result;
        }

        public IQueryable<Role> GetAllRoles()
        {
            IQueryable<Role> roles = _roleRepository.GetAll();
            if(roles!=null)
                return roles;
            return null;
        }

        public async Task<bool> AddRole(string name)
        {
            Role role = new Role()
            {
                Name = name
            };
            bool result = await _roleRepository.Add(role);
            return result;
        }

        public async Task<bool> RemoveRole(Role role)
        {

            bool result = await _roleRepository.Delete(role);
           
            return result;
        }

        public async Task<bool> UpdateRole(Role role)
        {
            bool result = await _roleRepository.Update(role);
           
            return result;
        }
    }
}
