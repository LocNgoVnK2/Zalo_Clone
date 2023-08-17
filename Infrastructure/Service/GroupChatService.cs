using Infrastructure.Entities;
using Infrastructure.Repository;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public interface IGroupChatService
    {
        Task<List<GroupChat>> GetAll();
        Task<GroupChat> GetGroupChatById(string id);
        Task<string> AddGroupChat(GroupChat groupChat, string imageByBase64);
        Task<bool> RemoveGroupChat(string id);
        Task<bool> UpdateLeader(string id, string newLeader);

        Task<bool> UpdateImage(string id, string newImage);
    }
    public class GroupChatService : IGroupChatService
    {
        private readonly IGroupChatRepository _groupChatRepository;
        private readonly IGroupUserRepository _userRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IUserContactRepository userContactRepository;
        private readonly IUtils utils;
        public GroupChatService(IGroupChatRepository groupChatRepository,
        IGroupUserRepository userRepository,
        IContactRepository contactRepository,
        IUtils utils,
        IUserContactRepository userContactRepository)
        {
            _groupChatRepository = groupChatRepository;
            _userRepository = userRepository;
            _contactRepository = contactRepository;
            this.userContactRepository = userContactRepository;
            this.utils = utils;
        }
        public async Task<string> AddGroupChat(GroupChat groupChat, string imageByBase64)
        {
            var transaction = await _groupChatRepository.BeginTransaction();
            do
            {
                groupChat.Id = utils.GenerateRandomString(64);
            } while (await _groupChatRepository.GetById(groupChat.Id) != null);
            if (!string.IsNullOrEmpty(imageByBase64))
            {
                groupChat.Image = Convert.FromBase64String(imageByBase64);
            }
            bool result = await _contactRepository.Add(new Contact()
            {
                Id = groupChat.Id,
                Avatar = groupChat.Image,
                ContactName = groupChat.Name
            });
            if (result)
            {

                result = await _groupChatRepository.Add(groupChat);
            }
            if (result)
            {
                var contact = new UserContact()
                {
                    UserId = groupChat.Leader,
                    ContactId = groupChat.Id
                };
                result = await userContactRepository.Add(contact);
                GroupUser user = new()
                {
                    IdGroup = groupChat.Id,
                    IdUser = groupChat.Leader,

                    JoinDate = DateTime.Now,
                    IdGroupRole = 0
                };

                result = await _userRepository.Add(user);

            }
            if (!result)
            {
                transaction.Rollback();
                transaction.Dispose();
                return null;
            }

            transaction.Commit();
            transaction.Dispose();
            string idGroup = groupChat.Id;
            return idGroup;
        }

        public async Task<List<GroupChat>> GetAll()
        {
            return await _groupChatRepository.GetAll().ToListAsync();
        }

        public async Task<GroupChat> GetGroupChatById(string id)
        {
            return await _groupChatRepository.GetById(id);
        }

        public async Task<bool> RemoveGroupChat(string id)
        {
            var contact = await _contactRepository.GetById(id);
            var grRemoveObj = await _groupChatRepository.GetById(id);
            bool result = await _contactRepository.Delete(contact);
            result = await _groupChatRepository.Delete(grRemoveObj);
            return result;

        }

        public async Task<bool> UpdateImage(string id, string newImage)
        {
            var grChangeImage = await _groupChatRepository.GetById(id);
            if (grChangeImage == null)
            {
                return false;
            }
            grChangeImage.Image = Convert.FromBase64String(newImage);
            var contact = await _contactRepository.GetById(id);
            contact.Avatar = grChangeImage.Image;

            bool result = await _contactRepository.Update(contact);
            return result;
        }

        public async Task<bool> UpdateLeader(string id, string newLeader)
        {
            var grChangeLeader = await _groupChatRepository.GetById(id);
            grChangeLeader.Leader = newLeader;
            bool result = await _groupChatRepository.Update(grChangeLeader);
            return result;
        }
    }
}
