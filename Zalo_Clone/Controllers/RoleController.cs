using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zalo_Clone.ModelViews;

namespace Zalo_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService,IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _roleService.GetRole(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleModel model)
        {
            Role role = _mapper.Map<Role>(model);
            var result = await _roleService.AddRole(role);
            if (!result)
                return BadRequest("Failed to add role.");

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole(RoleModel model)
        {
            Role role = _mapper.Map<Role>(model);
            var result = await _roleService.UpdateRole(role);
            if (!result)
                return BadRequest("Failed to update role.");

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveRole(RoleModel model)
        {
            Role role = _mapper.Map<Role>(model);
            var result = await _roleService.RemoveRole(role);
            if (!result)
                return BadRequest("Failed to remove role.");

            return Ok();
        }
    }
}
