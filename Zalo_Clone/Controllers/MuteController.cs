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
    public class MuteController : ControllerBase
    {
        private readonly IMuteService _muteService;
        private readonly IMapper _mapper;

        public MuteController(IMuteService muteService, IMapper mapper)
        {
            _muteService = muteService;
            _mapper = mapper;
        }

        [HttpPost("AddMuteGroup")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMuteGroup(MuteGroupModel model)
        {
            MuteGroup muteGroup = _mapper.Map<MuteGroup>(model);
            bool result = await _muteService.addMuteGroup(muteGroup);
            if (result)
            {
                return Ok("Mute group added successfully");
            }
            else
            {
                return BadRequest("Failed to add mute group");
            }
        }
        [HttpPost("AddMuteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMuteUser(MuteUserModel model)
        {
            MuteUser muteUser = _mapper.Map<MuteUser>(model);
            bool result = await _muteService.addMuteUser(muteUser);
            if (result)
            {
                return Ok("Mute user added successfully");
            }
            else
            {
                return BadRequest("Failed to add mute group");
            }
        }
        [HttpDelete("RemoveMuteGroup")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveMuteGroup(string userId, string groupId)
        {
            bool result = await _muteService.removeMuteGroup(userId, groupId);
            if (result)
            {
                return Ok("Mute group removed successfully");
            }
            else
            {
                return BadRequest("Failed to remove mute user");
            }
        }
        [HttpDelete("RemoveMuteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveMuteUser(string userSrc, string userDes)
        {
            bool result = await _muteService.removeMuteUser(userSrc, userDes);
            if (result)
            {
                return Ok("Mute user removed successfully");
            }
            else
            {
                return BadRequest("Failed to remove mute user");
            }
        }
        [HttpPut("UpdateMuteGroup")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMuteGroup(MuteGroupModel model)
        {
            MuteGroup muteGroup = _mapper.Map<MuteGroup>(model);
            bool result = await _muteService.updateMuteGroup(muteGroup);
            if (result)
            {
                return Ok("Mute group updated successfully");
            }
            else
            {
                return BadRequest("Failed to update mute group");
            }
        }
        [HttpPut("UpdateMuteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMuteUser(MuteUserModel model)
        {
            MuteUser muteUser = _mapper.Map<MuteUser>(model);
            bool result = await _muteService.updateMuteUser(muteUser);
            if (result)
            {
                return Ok("Mute user updated successfully");
            }
            else
            {
                return BadRequest("Failed to update mute user");
            }
        }
    }
}
