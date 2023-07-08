using Infrastructure.Entities;
using Infrastructure.Repository;

namespace Infrastructure.Service
{
    public interface IUserRoleService
    {
        Task<Role> GetRoleOfUser(string userId);
        Task<bool> AddRoleToUser(UserRole userRole);
    }
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        public UserRoleService(IUserRoleRepository _userRoleRepository, IRoleRepository _roleRepository)
        {
            this._userRoleRepository = _userRoleRepository;
            this._roleRepository = _roleRepository;
        }

        public async Task<Role> GetRoleOfUser(string userId)
        {

            var role = await _userRoleRepository.GetById(userId);
            return await _roleRepository.GetById(role.RoleId);
        }
        public async Task<bool> AddRoleToUser(UserRole userRole)
        {
            var result = await _userRoleRepository.Add(userRole);
            return result;
        }

    }
}
