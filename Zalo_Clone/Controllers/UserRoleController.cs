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
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService userRoleService;
        private readonly IMapper mapper;
        public UserRoleController(IUserRoleService userRoleService, IMapper mapper)
        {
            this.userRoleService = userRoleService;
            this.mapper = mapper;

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRoleToUser(UserRoleModel userRoleModel)
        {
            var userRole = mapper.Map<UserRole>(userRoleModel);
            bool result = await userRoleService.AddRoleToUser(userRole);
            if (result)
            {
                return Ok();
            }   
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoleOfUser(string userId){
            var userRole = await userRoleService.GetRoleOfUser(userId);
            return userRole == null ? NotFound() : Ok(userRole);
        }

    }
}
