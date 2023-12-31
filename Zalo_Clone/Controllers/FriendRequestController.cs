﻿using AutoMapper;
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
        private readonly IContactService contactService;
        private readonly IUserService   userService;
        private readonly IMapper mapper;
        public FriendRequestController(IUserService userService,IContactService contactService,IFriendRequestService friendRequestService, IFriendListService friendListService, IMapper mapper)
        {
            this.friendRequestService = friendRequestService;
            this.friendListService = friendListService;
            this.contactService = contactService;
            this.userService = userService;
            this.mapper = mapper;
        }
        [HttpPost("SendFriendRequest")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendFriendRequest(string userSenderId, string userReceiverId)
        {
            try
            {
                bool result = await friendRequestService.CreateFriendRequest(userSenderId, userReceiverId);
                if (result)
                {
                    return Ok("Request successfully");
                }
                else
                {
                    return NotFound("Are Friend");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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
            // update transation ở đây
            try
            {
                bool result = await friendRequestService.AcceptFriendRequest(userSenderId, userReceiverId);

                if (result)
                {
                    return Ok("Request successfully");
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
                List<FriendRequest> friendRequests = await friendRequestService.GetFriendRequestByIdForSender(userID);
                List<FriendRequestModel> friendRequestsModels = mapper.Map<List<FriendRequestModel>>(friendRequests);
                foreach (FriendRequestModel friendRequestModel in friendRequestsModels){
                    Contact contact = await contactService.GetContactData(friendRequestModel.User1);
                    User user = await userService.GetUserById(contact.Id);

                    friendRequestModel.UserName = contact.ContactName;
                    friendRequestModel.Email = user.Email;
                    if (contact.Avatar != null)
                    {
                        friendRequestModel.Avatar = Convert.ToBase64String(contact.Avatar);
                    }
                    else
                    {
                        friendRequestModel.Avatar = null;
                    }
                    
                }
                return Ok(friendRequestsModels);
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
                 List<FriendRequestModel> friendRequestsModels = mapper.Map<List<FriendRequestModel>>(friendRequests);
                foreach (FriendRequestModel friendRequestModel in friendRequestsModels){
                    Contact contact = await contactService.GetContactData(friendRequestModel.User2);
                    User user = await userService.GetUserById(contact.Id);
                    friendRequestModel.UserName = contact.ContactName;
                    friendRequestModel.Email=user.Email;
                    if (contact.Avatar != null)
                    {
                        friendRequestModel.Avatar = Convert.ToBase64String(contact.Avatar);
                    }
                    else
                    {
                        friendRequestModel.Avatar = null;
                    }
                    
                }
                return Ok(friendRequestsModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
          [HttpGet("CheckRequestingAddFriend")]
        public async Task<IActionResult> CheckRequestingAddFriend(string userSrc,string userDes)
        {
            try
            {
                bool result = await friendRequestService.CheckFriendRequesting(userSrc,userDes);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
