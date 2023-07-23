using Infrastructure.Entities;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Infrastructure.Service
{
    public interface IUserContactService
    {
        Task<List<UserContact>> GetContactsOfUserByTimeDesc(string userID);
    }
    public class UserContactService : IUserContactService
    {
        private readonly IUserDataRepository userDataRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IMessageReceipentRepository messageReceipentRepository;
        private readonly IMessageGroupRepository messageGroupRepository;
        private readonly IUserContactRepository userContactRepository;
        private readonly IGroupChatRepository groupChatRepository;
        private readonly IGroupUserRepository groupUserRepository;
        public UserContactService(IUserDataRepository userDataRepository,
        IMessageRepository messageRepository,
        IMessageReceipentRepository messageReceipentRepository,
        IMessageGroupRepository messageGroupRepository,
        IUserContactRepository userContactRepository,
        IGroupChatRepository groupChatRepository,
        IGroupUserRepository groupUserRepository)
        {
            this.userDataRepository = userDataRepository;
            this.messageRepository = messageRepository;
            this.messageGroupRepository = messageGroupRepository;
            this.messageReceipentRepository = messageReceipentRepository;
            this.userContactRepository = userContactRepository;
            this.groupChatRepository = groupChatRepository;
            this.groupUserRepository = groupUserRepository;
        }

        public async Task<List<UserContact>> GetContactsOfUserByTimeDesc(string userID)
        {
            var contacts = await userContactRepository.GetAll().Where(x => x.UserId.Equals(userID) || x.OtherUserId.Equals(userID)).ToListAsync();
            var messages = messageRepository.GetAll();
            var joinTable = contacts.Join(messages, x => x.LastMessageId, y => y.Id, (contacts, messages) => new { contacts, messages })
            .OrderByDescending(x => x.messages.SendTime);
            //Thêm contact group vào đây sau
            return contacts;
        }

    }
}
