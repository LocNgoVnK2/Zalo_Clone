using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zalo_Clone.Models;

namespace Zalo_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupChatController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGroupChatService _groupChatService;
        private readonly IGroupUserService _groupUserService;

        public GroupChatController(IMapper mapper, IGroupChatService groupChatService, IGroupUserService groupUserService)
        {
            _mapper = mapper;
            _groupChatService = groupChatService;
            _groupUserService = groupUserService;
        }
        [HttpPost("CreateGroupChat")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateGroupChat(GroupChatModel model)
        {
            GroupChat groupChat = _mapper.Map<GroupChat>(model);
            try
            {
                bool result = await _groupChatService.AddGroupChat(groupChat, model.imageByBase64);

                if (result)
                {
                    return Ok("Add group chat sussess");
                }
                else
                {
                    return BadRequest("cannot create group chat");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet("GetAllGroupChats")]
        public async Task<IActionResult> GetAllGroupChats()
        {
            List<GroupChat> groupChats = await _groupChatService.GetAll();

            List<GroupChatModel> result = _mapper.Map<List<GroupChatModel>>(groupChats);
            foreach (var groupChat in groupChats)
            {
                var groupChatModel = _mapper.Map<GroupChatModel>(groupChat);
                byte[] imageBytes = groupChat.Image!;
                if (imageBytes != null)
                {
                    string imageBase64 = Convert.ToBase64String(imageBytes);
                    groupChatModel.imageByBase64 = imageBase64;
                }
            }

            return Ok(result);

        }
        [HttpDelete("RemoveGroupChat{id}")]
        public async Task<IActionResult> RemoveGroupChat(string id)
        {
            var result = await _groupChatService.RemoveGroupChat(id);
            if (result)
                return Ok("Group chat have deleted");
            else
                return NotFound("Group chat not found");
        }
        [HttpPut("UpdateGroupChatLeader")]
        public async Task<IActionResult> UpdateGroupChatLeader(string id, string newLeader)
        {
            var result = await _groupChatService.UpdateLeader(id, newLeader);
            if (result)
                return Ok();
            else
                return NotFound("Group chat update fail");
        }
        [HttpPut("UpdateGroupImage")]
        public async Task<IActionResult> UpdateGroupImage(string id, string newImage)
        {
            var result = await _groupChatService.UpdateImage(id, newImage);
            if (result)
                return Ok();
            else
                return NotFound("Group chat update fail");
        }
    }
}
