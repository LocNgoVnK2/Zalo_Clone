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
        Task<Role> GetRole(int id);
        IQueryable<Role> GetAllRoles();
        Task<bool> AddRole(Role role);
        Task<bool> RemoveRole(Role role);
        Task<bool> UpdateRole(Role role);
    }
    public class RoleService:IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> GetRole(int id)
        {
            return await _roleRepository.GetById(id);
        }

        public IQueryable<Role> GetAllRoles()
        {
            return _roleRepository.GetAll();
        }

        public async Task<bool> AddRole(Role role)
        {
            return await _roleRepository.Add(role);
        }

        public async Task<bool> RemoveRole(Role role)
        {
            return await _roleRepository.Delete(role);
        }

        public async Task<bool> UpdateRole(Role role)
        {
            return await _roleRepository.Update(role);
        }
    }
}
