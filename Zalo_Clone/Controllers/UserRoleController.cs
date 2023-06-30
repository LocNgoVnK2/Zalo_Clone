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
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IMapper _mapper;

        public UserRoleController(IUserRoleService userRoleService,IMapper mapper)
        {
            _userRoleService = userRoleService;
            _mapper = mapper;
        }

        [HttpGet("GetUserRole")]
        public async Task<IActionResult> GetUserRole(string userid)
        {
            var role = await _userRoleService.GetRoleOfUser(userid);
            if (role == null)
                return NotFound();
            var roleModel = _mapper.Map<RoleModel>(role);
            return Ok(roleModel);
        }
        [HttpPost("AddUserRole")]
        public async Task<IActionResult> GetUserRole(string userid, string rolename)
        {
            var role = await _userRoleService.AddRoleToUser(userid, rolename);
            if (!role)
                return BadRequest();
            return Ok();
        }

    }
}
