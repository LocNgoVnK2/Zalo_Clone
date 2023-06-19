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
    public interface IUserDataService
    {
        Task<UserData> GetUserData(string id);
        Task<bool> AddUserData(UserData data);
        Task<bool> UpdateUserData(UserData data);
        Task<List<UserData>> GetAllUserData();
    }
    public class UserDataService : IUserDataService
    {
        private IUserDataRepository _repo;
        public UserDataService(IUserDataRepository repo)
        {
            this._repo = repo;
        }

        public async Task<bool> AddUserData(UserData data)
        {
            return await _repo.Add(data);
        }

        public async Task<List<UserData>> GetAllUserData()
        {
            return await _repo.GetAll().ToListAsync();
        }

        public async Task<UserData> GetUserData(string id)
        {
            return await _repo.GetById(id);
        }

        public async Task<bool> UpdateUserData(UserData data)
        {
            return await _repo.Update(data);
        }
    }
}
