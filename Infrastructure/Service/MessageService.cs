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

    public interface IMessageService
    {
        Task<bool> SendMessageToUser(Message message, string userReceive, List<string>? attachments);
        Task<bool> SendMessageToGroup(Message message, string groupReceive, List<string>? attachments);
        Task<bool> SendMessageToDoList(Message message, long taskId, List<string>? attachments);
        Task<bool> ReactToMessage(MessageReactDetail messageReactDetail);
        Task<bool> RecallMessage(long idMessage);
        
        int CountAllReactionInMessage(long idMessage);
        int CountReactionInMessage(long idMessage, int idReaction);
        Task<List<Reaction>> GetReactionsInMessage(long idMessage);
        Task<List<Reaction>> GetTop3ReactionsInMessage(long idMessage);

    }

    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IMessageReceipentRepository _messageReceipentRepo;
        private readonly IMessageAttachmentRepository _messageAttachmentRepo;
        private readonly IMessageGroupRepository _messageGroupRepo;
        private readonly IMessageToDoListRepository _messageToDoListRepo;
        private readonly IMessageReactDetailRepository _messageReactDetailRepo;
        private readonly IReactionRepository _reactionRepo;
        public MessageService(IMessageRepository messageRepo,
            IMessageReceipentRepository messageReceipentRepo,
            IMessageAttachmentRepository messageAttachmentRepo,
            IMessageGroupRepository messageGroupRepo,
            IMessageReactDetailRepository messageReactDetailRepository,
            IReactionRepository reactionRepository,
            IMessageToDoListRepository messageToDoListRepo)
        {
            this._messageRepo = messageRepo;
            this._messageReceipentRepo = messageReceipentRepo;
            this._messageAttachmentRepo = messageAttachmentRepo;
            this._messageGroupRepo = messageGroupRepo;
            _messageReactDetailRepo = messageReactDetailRepository;
            _reactionRepo = reactionRepository;
            _messageToDoListRepo = messageToDoListRepo;
        }

        public int CountAllReactionInMessage(long idMessage)
        {
            int count = _messageReactDetailRepo.GetAll().Where(m => m.MessageId == idMessage).Count();
            return count;
        }

        public int CountReactionInMessage(long idMessage, int idReaction)
        {
            var rs = _messageReactDetailRepo.GetAll().Where(x=> x.MessageId.Equals(idMessage) && x.ReactId.Equals(idReaction)).Count();
            return rs;
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
            if(! await _messageReactDetailRepo.Add(messageReactDetail))
            {
                transaction.Rollback();
                transaction.Dispose();
                return false;
            }
            transaction.Commit();
            transaction.Dispose() ;
            return true;
            
        }

        public async Task<bool> RecallMessage(long idMessage)
        {
            var message = await _messageRepo.GetById(idMessage);
            message.IsRecall = true;
            var transaction = await _messageRepo.BeginTransaction();
            if(! await _messageRepo.Update(message))
            {
                transaction.Rollback();
                transaction?.Dispose();
                return false;
            }
            transaction.Commit() ;
            transaction.Dispose() ; return true;

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

        public async Task<bool> SendMessageToGroup(Message message, string groupReceive, List<string>? attachments)
        {
            var transaction = await _messageRepo.BeginTransaction();

            bool result = await _messageRepo.Add(message);
            var messageGroup = new MessageGroup()
            {
                Id = message.Id,
                GroupReceive = groupReceive
               
            };
            result = await _messageGroupRepo.Add(messageGroup);
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


        public async Task<bool> SendMessageToUser(Message message, string userReceive, List<string>? attachments)
        {
            var transaction = await _messageRepo.BeginTransaction();

            bool result = await _messageRepo.Add(message);
            var messageReceipent = new MessageReceipent()
            {
                Id = message.Id,
                Receiver = userReceive
            };
            result = await _messageReceipentRepo.Add(messageReceipent);
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

    }
}