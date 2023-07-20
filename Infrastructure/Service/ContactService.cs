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
    public interface IContactService
    {
        Task<List<BlockList>> GetBlockListOfUser(string userID);
        Task<bool> Block(string userSrcId,string userDesId);
        Task<bool> UnBlock(string userSrcId, string userDesId);
        
    }
    public class ContactService : IContactService
    {
        private IBlockListRepository _repo;
        public ContactService(IBlockListRepository repo)
        {
            this._repo = repo;
        }

        public async Task<bool> Block(string userSrcId, string userDesId)
        {
            IQueryable<BlockList> block = _repo.GetAll().Where(o => o.UserSrcId.Equals(userSrcId) && o.UserDesId.Equals(userDesId));
            if(block.Count() > 0)
            {
                return false;
            }
            BlockList record = new BlockList()
            {
                BlockDate = DateTime.Now,
                UserSrcId = userSrcId,
                UserDesId = userDesId
            };
            return await _repo.Add(record);
        }

        public async Task<List<BlockList>> GetBlockListOfUser(string userID)
        {
            return await _repo.GetAll().Where(o => o.UserSrcId.Equals(userID)).ToListAsync();
        }

        public async Task<bool> UnBlock(string userSrcId, string userDesId)
        {
            BlockList block;
            try
            {
                block = _repo.GetAll().Single(o => o.UserSrcId == userSrcId && o.UserDesId == userDesId);
            }
            catch (Exception ex)
            {
                return false;
            }
            return await _repo.Delete(block);
            
            
        }
    }
}
