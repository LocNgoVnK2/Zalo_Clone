using Infrastructure.Entities;
using Infrastructure.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class UserRoleService 
    { }
        /*
        public interface IUserRoleService
        {
            Task<Role> GetRoleOfUser(string userId);
            Task<bool> AddRoleToUser(string userId, string roleName);
        }
        public class UserRoleService : IUserRoleService
        {


            public UserRoleService(UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Role> GetRoleOfUser(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var roles = await _userManager.GetRolesAsync(user);
                return roles.Select(x => new Role()).Single();
            }
            public async Task<bool> AddRoleToUser(string userId,string roleName)
            {

                    var user = await _userManager.FindByIdAsync(userId);
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    return true;



            }
        */
    
}
