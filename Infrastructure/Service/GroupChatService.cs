using Infrastructure.Entities;
using Infrastructure.Repository;
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
        Task<bool> AddGroupChat(GroupChat groupChat,string imageByBase64);
        Task<bool> RemoveGroupChat(string id);
        Task<bool> UpdateLeader(string id, string newLeader);

        Task<bool> UpdateImage(string id, string newImage);
    }
    public class GroupChatService : IGroupChatService
    {
        private readonly IGroupChatRepository _groupChatRepository;
        public GroupChatService(IGroupChatRepository groupChatRepository)
        {
            _groupChatRepository = groupChatRepository;
        }
        public async Task<bool> AddGroupChat(GroupChat groupChat, string imageByBase64)
        {
            if (imageByBase64 != null)
            {
                groupChat.Image = Convert.FromBase64String(imageByBase64);
            }
            else
            {
                groupChat.Image = new byte[0];
            }
            bool result = await _groupChatRepository.Add(groupChat);
            return result;
        }

        public async Task<List<GroupChat>> GetAll()
        {
            return await _groupChatRepository.GetAll().ToListAsync();
        }

        public async Task<bool> RemoveGroupChat(string id)
        {
            var grRemoveObj = await _groupChatRepository.GetById(id);
            bool result = await _groupChatRepository.Delete(grRemoveObj);
            return result;

        }

        public async Task<bool> UpdateImage(string id, string newImage)
        {
            var grChangeImage = await _groupChatRepository.GetById(id);
            if (grChangeImage != null)
            {
                grChangeImage.Image = Convert.FromBase64String(newImage);
            }
            else
            {
                grChangeImage.Image = new byte[0];
            }
                bool result = await _groupChatRepository.Update(grChangeImage);
            return result;
        }

        public async Task<bool> UpdateLeader(string id,string newLeader)
        {
            var grChangeLeader = await _groupChatRepository.GetById(id);
            grChangeLeader.Leader = newLeader;
            bool result = await _groupChatRepository.Update(grChangeLeader);
            return result;
        }
    }
}
