using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zalo_Clone.Models;

namespace Zalo_Clone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDataController : ControllerBase
    {
        private readonly IUserDataService _userDataService;
        private readonly IMapper mapper;
        public UserDataController(IUserDataService userDataService,IMapper mapper)
        {
            this._userDataService = userDataService;
            this.mapper = mapper;   
        }
        [HttpGet("id")]
        [ProducesResponseType(typeof(ReactionModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {
            Task<UserData> userdata = _userDataService.GetUserData(id);
            UserData userDataAwait = await userdata;
            if (userDataAwait != null)
            {
                return Ok(mapper.Map<UserDataModel>(userDataAwait));
            }
            return NotFound();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(UserDataModel userdataModel)
        {
            if(userdataModel.Id == 0)
            {
                return BadRequest();
            }
            UserData userdata = mapper.Map<UserData>(userdataModel);
            await _userDataService.AddUserData(userdata);
            return CreatedAtAction(nameof(GetByID), new {id = userdataModel.Id}, userdataModel);
        }
        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, UserDataModel userdataModel)
        {
            if(id == 0 || id != userdataModel.Id) {
                return BadRequest();
            }
            UserData userData = mapper.Map<UserData>(userdataModel);
            await _userDataService.UpdateUserData(userData);
            return NoContent();
        }
        
    }
}