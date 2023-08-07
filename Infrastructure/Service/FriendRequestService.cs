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
        Task<bool> RemoveFriendRequest(string userSrcId, string userDesId);
        Task<bool> CreateFriendRequest(string userSrcId, string userDesId);
        Task<bool> AcceptFriendRequest(string userSrcId, string userDesId);

        Task<bool> CheckFriendRequesting(string userSrcId,string userDesId);

    }
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IFriendListRepository _friendListRepository;

        public FriendRequestService(IFriendRequestRepository _friendRequestRepository, IFriendListRepository _friendListRepository)
        {
            this._friendRequestRepository = _friendRequestRepository;
            this._friendListRepository = _friendListRepository;

        }

        public async Task<bool> AcceptFriendRequest(string userSrcId, string userDesId)
        {
            try
            {
                var transaction = await _friendListRepository.BeginTransaction();
                FriendRequest friendRequest = await _friendRequestRepository.GetAll()
                    .FirstOrDefaultAsync(o => o.User1.Equals(userSrcId) && o.User2.Equals(userDesId));
                if (friendRequest != null)
                {
                    friendRequest.AcceptDate = DateTime.Now;
                    bool result = await _friendRequestRepository.Update(friendRequest);

                    if (result)
                    {
                        bool areFriends = await _friendListRepository.GetAll()
              .AnyAsync(f => (f.User1 == userSrcId && f.User2 == userDesId) || (f.User1 == userDesId && f.User2 == userSrcId));

                        if (areFriends)
                        {
                            result = false;
                        }

                        FriendList newFriend = new FriendList()
                        {
                            User1 = userSrcId,
                            User2 = userDesId
                        };
                        result = await _friendListRepository.Add(newFriend);
                    }
                    if (!result)
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                        return false;
                    }
                    transaction.Commit();
                    transaction.Dispose();
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
                FriendRequest alreadyRequested = await _friendRequestRepository.GetAll().FirstOrDefaultAsync(u =>
                                                (u.User1.Equals(userSrcId) && u.User2.Equals(userDesId)) ||
                                                (u.User1.Equals(userDesId) && u.User2.Equals(userSrcId)));
                if (alreadyRequested == null)
                {
                    bool result = await _friendRequestRepository.Add(friendRequest);
                    return result;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FriendRequest>> GetFriendRequestByIdForReceiver(string userID)
        {
            var friendRequests = await _friendRequestRepository.GetAll().Where(o => o.User1.Equals(userID)).ToListAsync();
            return friendRequests.ToList();
        }

        public async Task<List<FriendRequest>> GetFriendRequestByIdForSender(string userID)
        {
            var friendRequests = await _friendRequestRepository.GetAll().Where(o => o.User2.Equals(userID)).ToListAsync();
            return friendRequests.ToList();
        }

        public async Task<bool> RemoveFriendRequest(string userSrcId, string userDesId)
        {
            FriendRequest friendRequest = await _friendRequestRepository.GetAll().FirstOrDefaultAsync(u => u.User1.Equals(userSrcId) && u.User2.Equals(userDesId));
            if (friendRequest != null)
            {
                bool result = await _friendRequestRepository.Delete(friendRequest);
                return result;
            }

            return false;
        }
        public async Task<bool> CheckFriendRequesting(string userSrcId, string userDesId)
        {
            FriendRequest alreadyRequested = await _friendRequestRepository.GetAll().FirstOrDefaultAsync(u =>
                                                           (u.User1.Equals(userSrcId) && u.User2.Equals(userDesId)) ||
                                                           (u.User1.Equals(userDesId) && u.User2.Equals(userSrcId)));
            if (alreadyRequested == null)
            {
                return false;
            }else{
                return true;
            }
        }


    }
}
