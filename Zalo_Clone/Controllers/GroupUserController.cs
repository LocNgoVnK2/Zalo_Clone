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
        private readonly IMapper _mapper;

        public GroupUserController(IGroupUserService groupUserService, IMapper mapper)
        {
            _groupUserService = groupUserService;
            _mapper = mapper;
        }

        [HttpGet("GetAllUsersInGroup")]
        public async Task<IActionResult> GetAllUsersInGroup(string groupId)
        {
            var groupUsers = await _groupUserService.GetAllUserInGroup(groupId);
            var result = _mapper.Map<List<GroupUserModel>>(groupUsers);
            return Ok(result);
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


        [HttpPost("RemoveGroupUser")]
        public async Task<IActionResult> RemoveGroupUser(string idGroup,string idUser)
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
