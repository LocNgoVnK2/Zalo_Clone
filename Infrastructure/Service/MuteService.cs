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
    public interface IMuteService
    {
        Task<bool> addMuteGroup(MuteGroup muteGroup);
        Task<bool> removeMuteGroup(string userId,string groupId);
        Task<bool> updateMuteGroup(MuteGroup muteGroup);

        Task<bool> addMuteUser(MuteUser muteUser);
        Task<bool> removeMuteUser(string userId, string receiverId);
        Task<bool> updateMuteUser(MuteUser muteUser);
    }
    public class MuteService : IMuteService
    {
        private readonly IMuteGroupRepository _muteGroupRepository;
        private readonly IMuteUserRepository _muteUserRepository;
        public MuteService(IMuteGroupRepository muteGroupRepository, IMuteUserRepository muteUserRepository)
        {
            _muteGroupRepository = muteGroupRepository;
            _muteUserRepository = muteUserRepository;
        }
        public async Task<bool> addMuteGroup(MuteGroup muteGroup)
        {
            return await _muteGroupRepository.Add(muteGroup);
        }

        public async Task<bool> addMuteUser(MuteUser muteUser)
        {
            return await _muteUserRepository.Add(muteUser);
        }

        public async Task<bool> removeMuteGroup(string userId, string groupId)
        {
            MuteGroup muteGroup = await _muteGroupRepository.GetAll().Where(x=>x.User.Equals(userId) && x.GroupId.Equals(groupId)).FirstOrDefaultAsync();
            return await _muteGroupRepository.Delete(muteGroup);
        }

        public async Task<bool> removeMuteUser(string userId, string receiverId)
        {
            MuteUser muteUser = await _muteUserRepository.GetAll().Where(x => x.User.Equals(userId) && x.Receiver.Equals(receiverId)).FirstOrDefaultAsync();
            return await _muteUserRepository.Delete(muteUser);
        }

        public async Task<bool> updateMuteGroup(MuteGroup muteGroup)
        {
            return await _muteGroupRepository.Update(muteGroup);

        }

        public async Task<bool> updateMuteUser(MuteUser muteUser)
        {
            return await _muteUserRepository.Update(muteUser);
        }
    }
}
