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

        public interface IFriendRequestService
        {
            Task<List<FriendRequest>> GetFriendRequestByIdForSender(string userID);
            Task<List<FriendRequest>> GetFriendRequestByIdForReceiver(string userID);
            Task<bool> RemoveFriendRequest(long id);
            Task<bool> CreateFriendRequest(string userSrcId, string userDesId);
            Task<bool> AcceptFriendRequest(string userSrcId, string userDesId);
            
    }
        public class FriendRequestService : IFriendRequestService
        {
            private readonly IFriendRequestRepository _repo;
           
            public FriendRequestService(IFriendRequestRepository repo)
            {
                this._repo = repo;
             
            }

        public async Task<bool> AcceptFriendRequest(string userSrcId, string userDesId)
        {
            try
            {
                FriendRequest friendRequest = await _repo.GetAll()
                    .FirstOrDefaultAsync(o => o.User1.Equals(userSrcId) && o.User2.Equals(userDesId));

                if (friendRequest != null)
                {
                    friendRequest.AcceptDate = DateTime.Now;
                    bool result = await _repo.Update(friendRequest);
                    return result;
                }
                else
                {
                    throw new Exception("Friend request not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateFriendRequest(string userSrcId, string userDesId)
        {
            try
            {
                FriendRequest friendRequest = new FriendRequest()
                {
                    User1 = userSrcId,
                    User2 = userDesId,
                    RequestDate = DateTime.Now
                };
                FriendRequest alreadyRequested = await _repo.GetAll().FirstOrDefaultAsync(u =>
                                                (u.User1.Equals(userSrcId) && u.User2.Equals(userDesId)) ||
                                                (u.User1.Equals(userDesId) && u.User2.Equals(userSrcId)));
                if (alreadyRequested == null)
                {
                    bool result = await _repo.Add(friendRequest);
                    return result;
                }
                else
                {
                    return false;
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FriendRequest>> GetFriendRequestByIdForReceiver(string userID)
        {
            var friendRequests = await _repo.GetAll().Where(o => o.User1.Equals(userID)).ToListAsync();
            return friendRequests.ToList();
        }

        public async Task<List<FriendRequest>> GetFriendRequestByIdForSender(string userID)
        {
            var friendRequests = await _repo.GetAll().Where(o => o.User2.Equals(userID)).ToListAsync();
            return friendRequests.ToList();
        }

        public async Task<bool> RemoveFriendRequest(long id)
        {
            FriendRequest friendRequest = await _repo.GetById(id);
            if (friendRequest != null)
            {
                bool result = await _repo.Delete(friendRequest);
                return result;
            }

            return false;
        }
    }
}
