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
using static System.Collections.Specialized.BitVector32;

namespace Infrastructure.Service
{

    public interface IMessageService
    {
        Task<bool> SendMessageToContact(Message message, string contactId, List<string>? attachments);
        Task<bool> SendMessageToDoList(Message message, long taskId, List<string>? attachments);
        Task<bool> ReactToMessage(MessageReactDetail messageReactDetail);
        Task<bool> RecallMessage(long idMessage);

        int CountAllReactionInMessage(long idMessage);
        int CountReactionInMessage(long idMessage, int idReaction);
        Task<List<Reaction>> GetReactionsInMessage(long idMessage);
        Task<List<Reaction>> GetTop3ReactionsInMessage(long idMessage);
        Task<List<Message>> GetMessagesOfGroup(string groupId);
        Task<List<MessageAttachment>> GetAttachmentsOfMessage(long idMessage);

        Task<List<Message>> GetMessagesOfUsersContact(string userId, string contactId);
        Task<List<Message>> GetMessagesFromToDoList(long todoId);
        Task<Message> GetMessageById(long id);
        Task<bool> IsThereNewMessage(long lastMessageId, UserContact contact);
        Task<List<Contact>> GetContactsOfUnNotifiedMessage(string userId);
        Task<bool> ChangeMessageStatus(long messageId, string userReceiveId, MessageStatus status);

    }

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IMessageGroupRepository _messageGroupRepo;
        private readonly IMessageReceipentRepository _messageReceipentRepo;
        private readonly IMessageAttachmentRepository _messageAttachmentRepo;
        private readonly IMessageToDoListRepository _messageToDoListRepo;
        private readonly IMessageReactDetailRepository _messageReactDetailRepo;
        private readonly IReactionRepository _reactionRepo;
        private readonly IUserContactRepository userContactRepository;
        private readonly IGroupChatRepository groupChatRepository;
        private readonly IContactRepository contactRepository;
        private readonly IGroupUserRepository groupUserRepository;
        public MessageService(IMessageRepository messageRepo,
            IMessageGroupRepository messageGroupRepository,
            IMessageReceipentRepository messageReceipentRepository,
            IMessageAttachmentRepository messageAttachmentRepo,
            IMessageReactDetailRepository messageReactDetailRepository,
            IReactionRepository reactionRepository,
            IMessageToDoListRepository messageToDoListRepo,
            IUserContactRepository userContactRepository,
            IGroupChatRepository groupChatRepository,
            IContactRepository contactRepository,
            IGroupUserRepository groupUserRepository
            )
        {
            this._messageRepo = messageRepo;
            this._messageAttachmentRepo = messageAttachmentRepo;
            this._messageGroupRepo = messageGroupRepository;
            this._messageReceipentRepo = messageReceipentRepository;
            _messageReactDetailRepo = messageReactDetailRepository;
            _reactionRepo = reactionRepository;
            _messageToDoListRepo = messageToDoListRepo;
            this.userContactRepository = userContactRepository;
            this.groupChatRepository = groupChatRepository;
            this.contactRepository = contactRepository;
            this.groupUserRepository = groupUserRepository;
        }

        public int CountAllReactionInMessage(long idMessage)
        {
            int count = _messageReactDetailRepo.GetAll().Where(m => m.MessageId == idMessage).Count();
            return count;
        }

        public int CountReactionInMessage(long idMessage, int idReaction)
        {
            var rs = _messageReactDetailRepo.GetAll().Where(x => x.MessageId.Equals(idMessage) && x.ReactId.Equals(idReaction)).Count();
            return rs;
        }


        public async Task<List<MessageAttachment>?> GetAttachmentsOfMessage(long idMessage)
        {
            var rs = _messageAttachmentRepo.GetAll().Where(x => x.Id == idMessage);
            if (!rs.Any())
                return null;
            return await rs.ToListAsync();
        }

        public async Task<List<Reaction>> GetReactionsInMessage(long idMessage)
        {
            var reactions = _messageReactDetailRepo.GetAll().Where(x => x.MessageId == idMessage).Select(x => x.ReactId).Distinct().ToList();
            List<Reaction> result = new List<Reaction>();
            foreach (var reactionId in reactions)
            {
                var reaction = await _reactionRepo.GetById(reactionId)!;
                result.Add(reaction);
            }
            return result;
        }

        public async Task<List<Reaction>> GetTop3ReactionsInMessage(long idMessage)
        {
            var reactions = _messageReactDetailRepo.GetAll().Where(x => x.MessageId == idMessage)
                .GroupBy(x => x.ReactId).Select(grp => new { ReactId = grp.Key, Count = grp.Count() }).OrderByDescending(x => x.Count).Take(3);
            List<int> reactionList = reactions.Select(x => x.ReactId).ToList();
            List<Reaction> result = new List<Reaction>();
            foreach (var reactionId in reactionList)
            {
                var reaction = await _reactionRepo.GetById(reactionId)!;
                result.Add(reaction);
            }
            return result;
        }


        public async Task<bool> ReactToMessage(MessageReactDetail messageReactDetail)
        {
            var transaction = await _messageRepo.BeginTransaction();
            if (!await _messageReactDetailRepo.Add(messageReactDetail))
            {
                transaction.Rollback();
                transaction.Dispose();
                return false;
            }
            transaction.Commit();
            transaction.Dispose();
            return true;

        }

        public async Task<bool> RecallMessage(long idMessage)
        {
            var message = await _messageRepo.GetById(idMessage);
            message.IsRecall = true;
            var transaction = await _messageRepo.BeginTransaction();
            if (!await _messageRepo.Update(message))
            {
                transaction.Rollback();
                transaction?.Dispose();
                return false;
            }
            transaction.Commit();
            transaction.Dispose(); return true;

        }

        public async Task<bool> SendMessageToDoList(Message message, long taskId, List<string>? attachments)
        {
            var transaction = await _messageRepo.BeginTransaction();

            bool result = await _messageRepo.Add(message);
            var messageToDoList = new MessageToDoList()
            {
                Id = message.Id,
                TaskId = taskId

            };
            result = await _messageToDoListRepo.Add(messageToDoList);
            if (attachments != null || attachments.Count > 0)
            {
                foreach (var attachmentString in attachments)
                {
                    var attachment = new MessageAttachment()
                    {
                        Id = message.Id,
                        Attachment = Convert.FromBase64String(attachmentString)
                    };
                    result = await _messageAttachmentRepo.Add(attachment);
                }
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





        public async Task<List<Message>> GetMessagesOfUsersContact(string userId, string contactId)
        {
            var isContact = userContactRepository.GetAll().Where(x => (x.UserId.Equals(userId) && x.ContactId.Equals(contactId))
            || (x.UserId.Equals(contactId) && x.ContactId.Equals(userId))).FirstOrDefault() != null;
            if (!isContact)
                return null;

            var isGroupContact = (await groupChatRepository.GetById(contactId)) != null;
            if (isGroupContact)
            {
                return await GetMessagesOfGroup(contactId);
            }
            var messages = _messageRepo.GetAll();
            var messageContacts = _messageReceipentRepo.GetAll();
            var joinTable = messages.Join(messageContacts, x => x.Id, y => y.MessageId, (messages, messageContacts) => new { messages, messageContacts })
                .Where(x => (x.messages.Sender.Equals(userId) && x.messageContacts.UserId.Equals(contactId))
                || (x.messages.Sender.Equals(contactId) && x.messageContacts.UserId.Equals(userId)))
                .Select(x => x.messages)
                .ToListAsync();
            return await joinTable;


        }

        public async Task<List<Message>> GetMessagesFromToDoList(long todoId)
        {
            var rs = await _messageToDoListRepo.GetAll().Where(x => x.TaskId.Equals(todoId)).Select(x => x.Id).ToListAsync();
            List<Message> messages = new List<Message>();
            foreach (var r in rs)
            {
                Message message = await _messageRepo.GetById(r);
                messages.Add(message);
            }
            return messages;
        }

        public async Task<Message> GetMessageById(long id)
        {
            return await _messageRepo.GetById(id);
        }

        public async Task<bool> SendMessageToContact(Message message, string contactId, List<string>? attachments)
        {
            var transaction = await _messageRepo.BeginTransaction();
            if (await contactRepository.GetById(contactId) == null)
            {
                transaction.Rollback();
                transaction.Dispose();
                return false;
            }
            bool isGroupContact = await groupChatRepository.GetById(contactId) != null;
            bool result = await _messageRepo.Add(message);
            if (isGroupContact)
            {
                result = await _messageGroupRepo.Add(new MessageGroup()
                {
                    MessageId = message.Id,
                    GroupId = contactId
                });
                var groupUsers = groupUserRepository.GetAll().Where(x => x.IdGroup!.Equals(contactId)).Select(x => x.IdUser).ToList();
                foreach (var user in groupUsers)
                {
                    result = await _messageReceipentRepo.Add(new MessageReceipent()
                    {
                        MessageId = message.Id,
                        GroupId = contactId,
                        UserId = user!
                    });
                }
            }
            else
            {
                result = await _messageReceipentRepo.Add(new MessageReceipent()
                {
                    MessageId = message.Id,
                    UserId = contactId
                });
            }
            if (attachments != null || attachments?.Count > 0)
            {
                foreach (var attachmentString in attachments)
                {
                    var attachment = new MessageAttachment()
                    {
                        Id = message.Id,
                        Attachment = Convert.FromBase64String(attachmentString)
                    };
                    result = await _messageAttachmentRepo.Add(attachment);
                }
            }
            var contact = userContactRepository.GetAll().FirstOrDefault(x => (x.UserId.Equals(message.Sender) && x.ContactId.Equals(contactId)) || x.UserId.Equals(contactId) && x.ContactId.Equals(message.Sender));
            if (contact == null)
                result = await userContactRepository.Add(new UserContact()
                {
                    UserId = message.Sender,
                    ContactId = contactId

                });
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

        public async Task<List<Message>> GetMessagesOfGroup(string groupId)
        {
            var groupMessageId = _messageGroupRepo.GetAll().Where(x => x.GroupId.Equals(groupId)).Select(x => x.MessageId);
            var messages = _messageRepo.GetAll();
            var joinTable = await messages.Join(groupMessageId, x => x.Id, y => y, (messages, groupMessageId) => new { messages, groupMessageId })
            .Select(x => x.messages).ToListAsync();
            return joinTable;

        }

        public async Task<bool> IsThereNewMessage(long lastMessageId, UserContact contact)
        {
            var lastMessage = (await GetMessagesOfUsersContact(contact.UserId, contact.ContactId)).LastOrDefault();
            return lastMessageId != lastMessage?.Id;
        }
        private List<long> GetListOfUnnotifiedMessageId(string userId)
        {
            return null;
        }
        public async Task<List<Contact>> GetContactsOfUnNotifiedMessage(string userId)
        {
            
            using var transaction = await _messageRepo.BeginTransaction();
            List<string> groupContactIds = new List<string>();
            List<long> messageReceipentIds = new List<long>();
            var unNotifiedMessages = _messageReceipentRepo.GetAll().Where(x => x.UserId.Equals(userId) && x.Status == MessageStatus.Sent).ToList();
            if (!unNotifiedMessages.Any())
            {
                
                return null;
            }

            //Lọc các id group vào list riêng 
            foreach (var messageReceipent in unNotifiedMessages)
            {
                if (!string.IsNullOrEmpty(messageReceipent.GroupId))
                {
                    groupContactIds.Add(messageReceipent.GroupId);
                }
                else
                {
                    messageReceipentIds.Add(messageReceipent.MessageId);
                }
            }
            //Lấy id của các sender người mà gửi tin nhắn cho user nhận truyền vào
            var messages = _messageRepo.GetAll().ToList();
            var messageJoinTable = messages.Join(messageReceipentIds, x => x.Id, y => y, (messages, messageReceipentIds) => new { messages, messageReceipentIds })
            .Select(x => x.messages.Sender).ToList();
            //Thêm các id của group vào list contacts.
            messageJoinTable.AddRange(groupContactIds);
            var contacts = contactRepository.GetAll().ToList();
            //Lấy thông tin của contact join vào
            var contactJoinTable = contacts.Join(messageJoinTable, x => x.Id, y => y, (contacts, messageJoinTable) => new { contacts, messageJoinTable })
            .Select(x => x.contacts).Distinct().ToList();


            foreach (var message in unNotifiedMessages)
            {
                if (!await ChangeMessageStatus(message.MessageId, userId, MessageStatus.Received))
                {
                    transaction.Rollback();
                    return null;
                }
            }
            transaction.Commit();
            return contactJoinTable;
        }
        public async Task<bool> ChangeMessageStatus(long messageId, string userReceiveId, MessageStatus status)
        {
            bool result = false;
            var messageReceipent = _messageReceipentRepo.GetAll().Where(x => x.MessageId.Equals(messageId) && x.UserId.Equals(userReceiveId)).FirstOrDefault();
            if (messageReceipent != null)
            {
                messageReceipent.Status = status;
                result = await _messageReceipentRepo.Update(messageReceipent);
                return result;
            }
            return false;

        }
    }
}