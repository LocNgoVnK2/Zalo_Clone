using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
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
        public MessageController(IMessageService messageService, IMapper mapper)
        {
            this.messageService = messageService;
            this.mapper = mapper;   
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
        [HttpPost("SendMessageToUser")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessageToUser(MessageReceipentModel model)
        {
            var message = mapper.Map<Message>(model);
            bool result = await messageService.SendMessageToUser(message,model.Receiver,model.AttachmentByBase64);
            if(result)
            {
                return NoContent();
            }
            return BadRequest();
        }
        [HttpPost("SendMessageToGroup")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessageToGroup(MessageGroupModel model)
        {
            var message = mapper.Map<Message>(model);
            bool result = await messageService.SendMessageToGroup(message, model.GroupReceive, model.AttachmentByBase64);//SendMessageToUser(message, model.GroupReceive, model.AttachmentByBase64);
            if (result)
            {
                return NoContent();
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
        public async Task<int> GetNumberOfReactionInMessage(long messageId,int reactionId)
        {
            int result = messageService.CountReactionInMessage(messageId,reactionId);
            return result;
        }
        [HttpGet("GetTotalNumberReactionInMessage")]
        public async Task<int> GetTotalNumberReactionInMessage(long messageId)
        {
            int result = messageService.CountAllReactionInMessage(messageId);
            return result;
        }
        [HttpGet("GetMessagesOfGroup")]
        public async Task<List<MessageGroupModel>> GetMessagesOfGroup(string groupId)
        {
            var messages = await messageService.GetMessagesFromGroup(groupId);
            List<MessageGroupModel> result = new List<MessageGroupModel>();
            foreach(var m in messages)
            {
                long idMessage = m.Id;
                MessageGroupModel messageGroupModel = mapper.Map<MessageGroupModel>(m);
                messageGroupModel.AttachmentByBase64 = new List<string>();
                List<MessageAttachment> attachmenets = await messageService.GetAttachmentsOfMessage(idMessage);
                if (attachmenets is null)
                {
                    result.Add(messageGroupModel);
                    continue;
                }
                    
                foreach(MessageAttachment attachment in attachmenets)
                {
                    string attachmentByBase64 = Convert.ToBase64String(attachment.Attachment);
                    messageGroupModel.AttachmentByBase64.Add(attachmentByBase64);
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
        public async Task<List<MessageReceipentModel>> GetMessagesOfContactUser(string userFirst,string userSec)
        {
            var messages = await messageService.GetMessagesOfUsersContact(userFirst, userSec);
            var result = new List<MessageReceipentModel>();
            foreach (var m in messages)
            {
                long idMessage = m.Id;

                MessageReceipentModel messageReceipentModel = mapper.Map<MessageReceipentModel>(m);
                messageReceipentModel.Receiver = messageReceipentModel.Sender.Equals(userFirst) ? userSec : userFirst;
                messageReceipentModel.AttachmentByBase64 = new List<string>();
                List<MessageAttachment> attachmenets = await messageService.GetAttachmentsOfMessage(idMessage);
                if (attachmenets is null)
                {
                    result.Add(messageReceipentModel);
                    continue;
                }

                foreach (MessageAttachment attachment in attachmenets)
                {
                    string attachmentByBase64 = Convert.ToBase64String(attachment.Attachment);
                    messageReceipentModel.AttachmentByBase64.Add(attachmentByBase64);
                }
                result.Add(messageReceipentModel);
            }
            return result;
        }
    }
}