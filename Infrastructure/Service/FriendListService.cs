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
    public interface IFriendListService
    {
        Task<List<FriendList>> GetFriendListOfUser(string userID);
        Task<bool> UnFriend(string user1, string user2);
        Task<bool> AddFriend(string userSrcId, string userDesId);

    }
    public class FriendListService : IFriendListService
    {
        private readonly IFriendListRepository _friendListRepository;
        private readonly IFriendRequestRepository _friendRequestRepository;

        public FriendListService(IFriendListRepository _friendListRepository,IFriendRequestRepository _friendRequestRepository)
        {
            this._friendListRepository = _friendListRepository;
            this._friendRequestRepository = _friendRequestRepository;
        }

        public async Task<bool> AddFriend(string user1, string user2)
        {
            bool areFriends = await _friendListRepository.GetAll()
                .AnyAsync(f => (f.User1 == user1 && f.User2 == user2) || (f.User1 == user2 && f.User2 == user1));

            if (areFriends)
            {
                return false;
            }

            FriendList newFriend = new FriendList()
            {
                User1 = user1,
                User2 = user2
            };
            return await _friendListRepository.Add(newFriend);
        }

        public async Task<List<FriendList>> GetFriendListOfUser(string userID)
        {
            var friendList= await _friendListRepository.GetAll().Where(f => f.User1.Equals(userID) || f.User2.Equals(userID)).ToListAsync();
            return friendList;
        }
        public async Task<bool> UnFriend(string userSrcId, string userDesId)
        {
            var transaction = await _friendListRepository.BeginTransaction();
            FriendList friend = await _friendListRepository.GetAll().FirstOrDefaultAsync(f=>(f.User1.Equals(userSrcId)&& f.User2.Equals(userDesId))|| (f.User1.Equals(userDesId) && f.User2.Equals(userSrcId)));
          
            bool result = await _friendListRepository.Delete(friend);
            if (result)
            {
                FriendRequest friendRequest = await _friendRequestRepository.GetAll().FirstOrDefaultAsync(u => u.User1.Equals(userSrcId) && u.User2.Equals(userDesId));
                result = await _friendRequestRepository.Delete(friendRequest);
               
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
    }
}