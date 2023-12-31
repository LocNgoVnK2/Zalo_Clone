using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Zalo_Clone.Models;
using Zalo_Clone.ModelViews;

namespace Zalo_Clone.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService messageService;
        private readonly IMapper mapper;
        private readonly IContactService contactService;
        public MessageController(IMessageService messageService
        ,IMapper mapper
        ,IContactService contactServices)
        {
            this.messageService = messageService;
            this.mapper = mapper;
            this.contactService = contactServices;
        }
        [HttpPost("RecallMessage")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RecallMessage(long idMessage)
        {

            bool result = await messageService.RecallMessage(idMessage);
            if (result)
            {
                return NoContent();
            }
            return BadRequest();
        }
        [HttpPost("SendMessageToContact")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessageToContact(MessageSendModel model)
        {
            var message = mapper.Map<Message>(model);
            message.SendTime = DateTime.Now;
            var attachmenets = mapper.Map<List<MessageAttachment>>(model.MessageAttachments);
            bool result = await messageService.SendMessageToContact(message, model.ContactId, attachmenets);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("SendMessageToDoList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessageToDoList(MessageToDoListModel model)
        {
            var message = mapper.Map<Message>(model);
            bool result = await messageService.SendMessageToDoList(message, model.TaskId, model.AttachmentByBase64);//SendMessageToUser(message, model.GroupReceive, model.AttachmentByBase64);
            if (result)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPost("ReactToMessage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReactToMessage(MessageReactModel model)
        {
            var message = mapper.Map<MessageReactDetail>(model);
            bool result = await messageService.ReactToMessage(message);
            if (result)
            {
                return NoContent();
            }
            return BadRequest();
        }
        [HttpGet("GetTop3ReactionInMessage")]
        public async Task<List<Reaction>> GetTop3ReactionTypeOfMessage(long messageId)
        {
            List<Reaction> reactions = await messageService.GetTop3ReactionsInMessage(messageId);
            return reactions;
        }
        [HttpGet("GetReactionsInMessage")]
        public async Task<List<Reaction>> GetReactionsInMessage(long messageId)
        {
            List<Reaction> reactions = await messageService.GetReactionsInMessage(messageId);
            return reactions;
        }
        [HttpGet("GetNumberOfReactionInMessage")]
        public async Task<int> GetNumberOfReactionInMessage(long messageId, int reactionId)
        {
            int result = messageService.CountReactionInMessage(messageId, reactionId);
            return result;
        }
        [HttpGet("GetTotalNumberReactionInMessage")]
        public async Task<int> GetTotalNumberReactionInMessage(long messageId)
        {
            int result = messageService.CountAllReactionInMessage(messageId);
            return result;
        }
        [HttpGet("GetMessagesOfGroup")]
        public async Task<List<MessageContactModel>> GetMessagesOfGroup(string groupId)
        {
            var messages = await messageService.GetMessagesOfGroup(groupId);
            List<MessageContactModel> result = new List<MessageContactModel>();
            foreach (var m in messages)
            {
                long idMessage = m.Id;
                MessageContactModel messageGroupModel = mapper.Map<MessageContactModel>(m);
                messageGroupModel.MessageAttachments = new List<MessageAttachmentModel>();
                List<MessageAttachment> attachments = await messageService.GetAttachmentsOfMessage(idMessage);
                if (attachments == null)
                {
                    result.Add(messageGroupModel);
                    continue;
                }

                foreach (MessageAttachment attachment in attachments)
                {
                    var messageAttachmentModel = mapper.Map<MessageAttachmentModel>(attachment);
                    string attachmentByBase64 = Convert.ToBase64String(attachment.Attachment!);
                    messageAttachmentModel.AttachmentByBase64 = attachmentByBase64;
                    messageGroupModel.MessageAttachments.Add(messageAttachmentModel);
                }
                result.Add(messageGroupModel);
            }
            return result;
        }
        [HttpGet("GetMessagesToDoList")]
        public async Task<List<MessageToDoListModel>> GetMessagesOfToDoList(long todoId)
        {
            var messages = await messageService.GetMessagesFromToDoList(todoId);
            List<MessageToDoListModel> result = new List<MessageToDoListModel>();
            foreach (var m in messages)
            {
                long idMessage = m.Id;
                MessageToDoListModel messageToDoModel = mapper.Map<MessageToDoListModel>(m);
                messageToDoModel.AttachmentByBase64 = new List<string>();
                List<MessageAttachment> attachmenets = await messageService.GetAttachmentsOfMessage(idMessage);
                if (attachmenets is null)
                {
                    result.Add(messageToDoModel);
                    continue;
                }

                foreach (MessageAttachment attachment in attachmenets)
                {
                    string attachmentByBase64 = Convert.ToBase64String(attachment.Attachment);
                    messageToDoModel.AttachmentByBase64.Add(attachmentByBase64);
                }
                result.Add(messageToDoModel);
            }
            return result;
        }

        [HttpGet("GetMessagesFromContactOfUser")]
        public async Task<IActionResult> GetMessagesOfContactUser(string userId, string contactId)
        {
            var messages = await messageService.GetMessagesOfUsersContact(userId, contactId);
            if (messages == null)
                return BadRequest();
            var result = new List<MessageContactModel>();
            foreach (var m in messages)
            {
                long idMessage = m.Id;
                MessageContactModel messageReceipentModel = mapper.Map<MessageContactModel>(m);
                messageReceipentModel.SenderName = (await contactService.GetContactData(messageReceipentModel.Sender)).ContactName;
                messageReceipentModel.ContactId = contactId;
                messageReceipentModel.ContactName = (await contactService.GetContactData(contactId)).ContactName;
                messageReceipentModel.MessageAttachments = new List<MessageAttachmentModel>();
                List<MessageAttachment> attachmenets = await messageService.GetAttachmentsOfMessage(idMessage);
                if (attachmenets == null)
                {   
                    result.Add(messageReceipentModel);
                    continue;
                }

                foreach (MessageAttachment attachment in attachmenets)
                {
                     var messageAttachmentModel = mapper.Map<MessageAttachmentModel>(attachment);
                    string attachmentByBase64 = Convert.ToBase64String(attachment.Attachment!);
                    messageAttachmentModel.AttachmentByBase64 = attachmentByBase64;
          
                    messageReceipentModel.MessageAttachments.Add(messageAttachmentModel);
                }
                result.Add(messageReceipentModel);
            }
            return Ok(result);
        }
        [HttpGet("GetContactsOfUnNotifiedMessage")]
        public async Task<IActionResult> GetContactsOfUnNotifiedMessage(string userId)
        {
            var timeOut = 15000;
            var frequency = 500;
            List<Contact>? contacts =  null;
        
            try
            {
                var waitTask = Task.Run(async () =>
                {
            
                    while (contacts == null)
                    {
                        contacts = await messageService.GetContactsOfUnNotifiedMessage(userId);
                        await Task.Delay(frequency);
                    }
                });

                if (waitTask != await Task.WhenAny(waitTask,
                        Task.Delay(timeOut)))
                    throw new TimeoutException();
            }
            catch (TimeoutException)
            {
                return NoContent();
            }
            return Ok(contacts);

        }

    }
}