using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Zalo_Clone.Models;

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
            bool result = await messageService.SendMessageToUser(message, model.GroupReceive, model.AttachmentByBase64);
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
    }
}