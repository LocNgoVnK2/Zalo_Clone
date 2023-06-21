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

    }
    
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IMessageReceipentRepository _messageReceipentRepo;
        private readonly IMessageAttachmentRepository _messageAttachmentRepo;
        public MessageService(IMessageRepository messageRepo, IMessageReceipentRepository messageReceipentRepo, IMessageAttachmentRepository messageAttachmentRepo)
        {
            this._messageRepo = messageRepo;
            this._messageReceipentRepo = messageReceipentRepo;
            this._messageAttachmentRepo = messageAttachmentRepo;
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
