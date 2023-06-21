using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Zalo_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
       
        private readonly IFriendListService friendListService;
        private readonly IFriendRequestService friendRequestService;
        private readonly IMapper mapper;
        public FriendController(IFriendRequestService friendRequestService,IFriendListService friendListService, IMapper mapper)
        {
            this.friendRequestService = friendRequestService;
            this.friendListService = friendListService;
            this.mapper = mapper;
        }
        [HttpGet("GetFriendByID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFriendByID(string userId)
        {
            try
            {
                List<FriendList> friendLists= await friendListService.GetFriendListOfUser(userId);
                if (friendLists!=null)
                {

                    return Ok(friendLists); 

                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Unfriend")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Unfriend(string userSenderId, string userReceiverId)
        {
            try
            {
                bool result = await friendListService.UnFriend(userSenderId, userReceiverId);
                if (result)
                {
                    
                    bool resultDeleteFriendRequest = await friendRequestService.RemoveFriendRequest(userSenderId, userReceiverId);
                    if (resultDeleteFriendRequest)
                    {
                        return Ok("Delete success");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
