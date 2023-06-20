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
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService friendRequestService;
        private readonly IFriendListService friendListService;
        private readonly IMapper mapper;
        public FriendRequestController(IFriendRequestService friendRequestService, IFriendListService friendListService, IMapper mapper)
        {
            this.friendRequestService = friendRequestService;
            this.friendListService = friendListService;
            this.mapper = mapper;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendFriendRequest(string userSenderId , string userReceiverId)
        {
            bool result = await friendRequestService.CreateFriendRequest(userSenderId, userReceiverId);
            if (result)
            {
                return Ok("Request successfully");
            }
            return BadRequest();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeniedFriendRequest(long Id)
        {
            bool result = await friendRequestService.RemoveFriendRequest(Id);
            if (result)
            {
                return Ok("Request successfully");
            }
            return BadRequest();
        }
        /*
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptFriendRequest(FriendRequestModel friendRequestModel)
        {
           

            return BadRequest();
        }
        */
    }
}
