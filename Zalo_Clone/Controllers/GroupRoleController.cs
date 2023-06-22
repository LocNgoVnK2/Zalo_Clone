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
    public class GroupRoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGroupRoleService _groupRoleService;

        public GroupRoleController(IMapper mapper, IGroupRoleService groupRoleService)
        {
            _mapper = mapper;
            _groupRoleService = groupRoleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroupRoles()
        {
            var groupRoles = await _groupRoleService.GetAll();
            var groupRoleModels = _mapper.Map<List<GroupRoleModel>>(groupRoles);
            return Ok(groupRoleModels);
        }

                [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddGroupRole(GroupRoleModel groupRoleModel)
        {
            var groupRole = _mapper.Map<GroupRole>(groupRoleModel);
            var result = await _groupRoleService.AddGroupRole(groupRole);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to add group role");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroupRole(GroupRoleModel groupRoleModel)
        {
            var groupRole = _mapper.Map<GroupRole>(groupRoleModel);
            var result = await _groupRoleService.UpdateGroupRole(groupRole);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to update group role");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveGroupRole(int id)
        {
            
            var result = await _groupRoleService.RemoveGroupRole(id);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to remove group role");
        }

    }
}
