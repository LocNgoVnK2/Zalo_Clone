using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IRoleService
    {
        Role GetRole(string id);
        IQueryable<Role> GetAllRoles();
        Task<bool> AddRole(string name);
        Task<bool> RemoveRole(Role role);
        Task<bool> UpdateRole(Role role);
    }
    public class RoleService:IRoleService
    {

        private readonly RoleManager<Role> _roleManager;
        private readonly IRoleRepository _roleRepository;
        private readonly ZaloDbContext _dbContext;
        public RoleService(RoleManager<Role> roleManager, ZaloDbContext context, IRoleRepository roleRepository)
        {
            _roleManager = roleManager;
            _dbContext = context;
            _roleRepository = roleRepository;
        }

        public Role GetRole(string id)
        {
            var result = _roleManager.Roles.Where(t => t.Id.Equals(id)).Single();
            return result;
        }

        public IQueryable<Role> GetAllRoles()
        {
            var result = _roleManager.Roles;
            if(result.Any())
                return result;
            return null;
        }

        public async Task<bool> AddRole(string name)
        {
            var exist = await _roleManager.RoleExistsAsync(name);
            if(!exist)
            {
                Role role = new Role();
                role.Name = name;
                await _roleManager.CreateAsync(role);
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveRole(Role role)
        {
            var exist = await _roleManager.RoleExistsAsync(role.Name);
            if (exist)
            {
                
                await _roleManager.DeleteAsync(role);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateRole(Role role)
        {
            var exist = await _roleManager.RoleExistsAsync(role.Name);
            if (exist)
            {
                await _roleManager.UpdateAsync(role);
                return true;
            }
            return false;
        }
    }
}
