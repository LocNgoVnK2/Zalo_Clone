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
    public class FriendController : ControllerBase
    {

        private readonly IFriendListService friendListService;
        private readonly IFriendRequestService friendRequestService;
        private readonly IUserService userService;
        private readonly IUserContactService userContactService;
        private readonly IContactService contactService;
        private readonly IMapper mapper;
        public FriendController (IContactService contactService,IUserService userService,IUserContactService userContactService, IFriendRequestService friendRequestService, IFriendListService friendListService, IMapper mapper)
        {
            this.friendRequestService = friendRequestService;
            this.friendListService = friendListService;
            this.userContactService = userContactService;
            this.userService = userService;
            this.contactService = contactService;
            this.mapper = mapper;
        }
        [HttpGet("GetFriendByID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFriendByID(string userId)
        {
            try
            {
                List<FriendList> friendLists = await friendListService.GetFriendListOfUser(userId);
                if (friendLists != null)
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
        [HttpGet("CheckIsFriend")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckIsFriend(string id, string emailSearched)
        {
            try
            {
                bool result = await friendListService.CheckFriend(id,emailSearched);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
                    return Ok("Delete success");
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("RecommandFriend")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RecommandFriend(string id)
        {
            try
            {
                var recommandUser =  await userService.GetRecommandUsers(id);
                List<UserInformationModel> models = new List<UserInformationModel>();
                foreach(var user in recommandUser){
                    Contact contact = await contactService.GetContactData(user.Id);
                    UserInformationModel model = new UserInformationModel();
                    model.PhoneNumber = user.PhoneNumber;
                    model.UserName = contact.ContactName;
                     if (contact.Avatar != null)
                    {
                        model.Avatar = Convert.ToBase64String(contact.Avatar);
                    }
                    else
                    {
                        contact.Avatar = null;
                    }
                    if (user.Background != null)
                    {
                        model.Background = Convert.ToBase64String(user.Background);
                    }
                    else
                    {
                        model.Background = null;
                    }
                    model.Gender = user.Gender;
                    model.DateOfBirth = user.DateOfBirth;
                    model.Email = user.Email;
                    // push model to models
                    models.Add(model);
                }
                return Ok(models);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
    }
}
