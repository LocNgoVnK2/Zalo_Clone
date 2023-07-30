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

        private readonly IMessageRepository messageRepository;
        private readonly IUserContactRepository userContactRepository;
        private readonly IGroupChatRepository groupChatRepository;
        private readonly IGroupUserRepository groupUserRepository;
        public UserContactService(
        IMessageRepository messageRepository,
        IUserContactRepository userContactRepository,
        IGroupChatRepository groupChatRepository,
        IGroupUserRepository groupUserRepository)
        {
            this.messageRepository = messageRepository;
            this.userContactRepository = userContactRepository;
            this.groupChatRepository = groupChatRepository;
            this.groupUserRepository = groupUserRepository;
        }

        public async Task<List<UserContact>> GetContactsOfUserByTimeDesc(string userID)
        {
            var contacts = await userContactRepository.GetAll().Where(x => x.UserId.Equals(userID) || x.ContactId.Equals(userID)).ToListAsync();
            foreach(var contact in contacts){
                if(contact.ContactId.Equals(userID)){
                    var temp = contact.UserId;
                    contact.UserId = userID;
                    contact.ContactId = temp;
                }
            }
            //var messages = messageRepository.GetAll();
            // var joinTable = contacts.Join(messages, x => x.LastMessageId, y => y.Id, (contacts, messages) => new { contacts, messages })
            // .OrderByDescending(x => x.messages.SendTime);
            //Thêm contact group vào đây sau
            return contacts;
        }

    }
}
