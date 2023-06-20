using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendCommonMessageToUser(MessageReceipentModel model)
        {
            var message = mapper.Map<Message>(model);
            bool result = await messageService.SendMessageToUser(message,model.Receiver,model.AttachmentByBase64);
            if(result)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}