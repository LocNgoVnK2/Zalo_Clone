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
    public class GroupUserController : ControllerBase
    {
        private readonly IGroupUserService _groupUserService;
        private readonly IGroupChatService _groupChatService;
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public GroupUserController(IContactService contactService, IGroupChatService groupChatService, IGroupUserService groupUserService, IMapper mapper)
        {
            _groupUserService = groupUserService;
            _groupChatService = groupChatService;
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpGet("GetAllUsersInGroup")]
        public async Task<IActionResult> GetAllUsersInGroup(string groupId)
        {
            var groupUsers = await _groupUserService.GetAllUserInGroup(groupId);
            List<ContactDataModel> userInGroup = new List<ContactDataModel>();
            if (groupUsers != null)
            {
                foreach (var user in groupUsers)
                {
                    Contact contact = await _contactService.GetContactData(user.IdUser);
                    ContactDataModel contactData = _mapper.Map<ContactDataModel>(contact);
                    userInGroup.Add(contactData);
                }
            }
            return Ok(userInGroup);
        }
        [HttpPost("AddGroupUser")]
        public async Task<IActionResult> AddGroupUser(GroupUserModel groupUserModel)
        {
            var groupUser = _mapper.Map<GroupUser>(groupUserModel);
            var success = await _groupUserService.AddGroupUser(groupUser);
            if (success)
            {
                return Ok("Group user added successfully.");
            }
            else
            {
                return BadRequest("Failed to add group user.");
            }
        }
        [HttpPost("AddManyGroupUser")]
        public async Task<IActionResult> AddManyGroupUser(GroupUserModel[] groupUserModel)
        {
            foreach (GroupUserModel groupUser in groupUserModel)
            {
                var groupUserToAdd = _mapper.Map<GroupUser>(groupUser);
                var success = await _groupUserService.AddGroupUser(groupUserToAdd);
                if (!success)
                {
                    return BadRequest("Failed to add group user.");
                }
            }
            return Ok(" Many group user added successfully.");
        }

        [HttpPost("RemoveGroupUser")]
        public async Task<IActionResult> RemoveGroupUser(string idGroup, string idUser)
        {
            var listGroup = await _groupChatService.GetAll();
            var isLeader = listGroup.Where(g => g.Id == idGroup && g.Leader == idUser).FirstOrDefault();
            if (isLeader == null)
            {
                var success = await _groupUserService.RemoveGroupUser(idGroup, idUser);
                if (success)
                {
                    return Ok("Group user removed successfully.");
                }
                else
                {
                    return BadRequest("Failed to remove group user.");
                }
            }
            else
            {
                return NoContent();
            }
        }
        [HttpPut("UpdateGroupUser")]
        public async Task<IActionResult> UpdateGroupUser(GroupUserModel groupUserModel)
        {
            var groupUser = _mapper.Map<GroupUser>(groupUserModel);
            var success = await _groupUserService.UpdateGroupUser(groupUser);
            if (success)
            {
                return Ok("Group user updated successfully.");
            }
            else
            {
                return BadRequest("Failed to update group user.");
            }
        }
    }
}
