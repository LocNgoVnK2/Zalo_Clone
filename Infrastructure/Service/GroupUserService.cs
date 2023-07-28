using Infrastructure.Entities;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IGroupUserService
    {
        Task<List<GroupUser>> GetAllUserInGroup(string groupId);
        Task<bool> AddGroupUser(GroupUser groupUser);
        Task<bool> RemoveGroupUser(string idGroup, string idUser);
        Task<bool> UpdateGroupUser(GroupUser groupUser);
        Task<List<GroupUser>> GetAllGroupsOfUser(string userId);
        Task<List<Contact>> GetGroupContactsOfUser(string userId);
    }
    public class GroupUserService : IGroupUserService
    {
        IGroupUserRepository repository;
        private readonly IGroupChatRepository groupChatRepository;
        private readonly IUserContactRepository contactRepository;
        public GroupUserService(IGroupUserRepository repository,
        IUserContactRepository contactRepository,
        IGroupChatRepository groupChatRepository) 
        {
            this.repository = repository;
            this.contactRepository = contactRepository;
            this.groupChatRepository = groupChatRepository;
        }
        public async Task<bool> AddGroupUser(GroupUser groupUser)
        {
            var contact = new UserContact(){
                UserId = groupUser.IdUser!,
                ContactId = groupUser.IdGroup!
            };
            bool result = await contactRepository.Add(contact);
            result = await repository.Add(groupUser);
            return result;
        }

        public async Task<List<GroupUser>> GetAllGroupsOfUser(string userId)
        {
            return await repository.GetAll().Where(x=>x.IdUser == userId).Select(x=>x).ToListAsync();
        }

        public async Task<List<GroupUser>> GetAllUserInGroup(string groupId)
        {
            return await repository.GetAll().Where(x=>x.IdGroup == groupId).Select(x=>x).ToListAsync();
        }

        public async Task<bool> RemoveGroupUser(string idGroup,string idUser)
        {
            GroupUser groupUser = (await repository.GetAll().Where(x=>x.IdUser.Equals(idUser) && x.IdGroup.Equals(idGroup)).FirstOrDefaultAsync())!;
            UserContact contact = contactRepository.GetAll().FirstOrDefault(x => x.UserId.Equals(idUser) && x.ContactId.Equals(idGroup))!;
            bool result = false;
            if(contact != null){
                result = await contactRepository.Delete(contact);
            }
            result = await repository.Delete(groupUser);
            return result;
        }

        public async Task<bool> UpdateGroupUser(GroupUser groupUser)
        {
            bool result = await repository.Update(groupUser);
            return result;
        }

        public async Task<List<Contact>> GetGroupContactsOfUser(string userId)
        {
            var contacts = contactRepository.GetAll().Where(a => a.UserId.Equals(userId)).Select(x => x.ContactId).ToList();
            List<Contact> result = new();
            foreach(var contact in contacts){
                var groupChat = await groupChatRepository.GetById(contact);
                var groupContact = new Contact(){
                    Id = groupChat.Id,
                    ContactName = groupChat.Name,
                    Avatar = groupChat.Image
                };
                result.Add(groupContact);
            }
            return result;
        }
    }
}
