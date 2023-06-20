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
        [HttpPost("SendFriendRequest")]
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

        [HttpDelete("DeniedFriendRequest")]
        public async Task<IActionResult> DeniedFriendRequest(string userSenderId, string userReceiverId)
        {
            bool result = await friendRequestService.RemoveFriendRequest(userSenderId, userReceiverId);
            if (result)
            {
                return Ok("Request successfully");
            }
            return BadRequest();
        }
        
        [HttpPost("AcceptFriendRequest")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptFriendRequest(string userSenderId, string userReceiverId)
        {
            try
            {
                bool result = await friendRequestService.AcceptFriendRequest(userSenderId, userReceiverId);
                if (result)
                {
                    bool newFriend = await friendListService.AddFriend(userSenderId, userReceiverId);
                    if (newFriend)
                    {
                        return Ok("Request successfully");
                    }
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetFriendRequestsByIdOfReceiver")]
        public async Task<IActionResult> GetFriendRequestsByIdOfReceiver(string userID)
        {
            try
            {
                var friendRequests = await friendRequestService.GetFriendRequestByIdForSender(userID);
                
                return Ok(friendRequests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetFriendRequestsByIdOfSender")]
        public async Task<IActionResult> GetFriendRequestsByIdOfSender(string userID)
        {
            try
            {
                var friendRequests = await friendRequestService.GetFriendRequestByIdForReceiver(userID);
                return Ok(friendRequests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
