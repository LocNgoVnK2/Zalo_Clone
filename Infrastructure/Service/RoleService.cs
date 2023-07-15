using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IRoleService
    {
        Task<Role> GetRole(string name);
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
        private string GenerateRandomId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var id = new string(Enumerable.Repeat(chars, 64)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return id;
        }
        public async Task<Role?> GetRole(string name)
        {
            var result = await _roleRepository.GetAll().Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
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
            if (await GetRole(name) != null)
            {
                return false;
            }
            string id = "";
            do
            {
                id = GenerateRandomId();
                var roles = await GetAllRoles().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
                if (roles == null)
                    break;
            } while (true);
            Role role = new Role()
            {
                Id = id,
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
