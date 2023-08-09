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
    public class SearchLogController : ControllerBase
    {
        private readonly ISearchLogService searchLogService;
        private readonly IContactService contactService;
        private readonly IUserService userService;

        public SearchLogController(ISearchLogService searchLogService, IContactService contactService, IUserService userService)
        {
            this.searchLogService = searchLogService;
            this.contactService = contactService;
            this.userService = userService;
        }
        [HttpPost("AddSearchLog")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSearchLog(string userSrc, string userDes)
        {
            bool result = await searchLogService.AddSearchLog(userSrc, userDes);

            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost("RemoveSearchLog")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveSearchLog(string userSrc, string userDes)
        {
            bool result = await searchLogService.DeleteSearchLog(userSrc, userDes);

            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet("GetRecentSearch")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRecentSearch(string userSrc)
        {

            try
            {
                var searchLogs = await searchLogService.GetSearchResent(userSrc);
                List<UserInformationModel> models = new List<UserInformationModel>();
                foreach (var user in searchLogs)
                {
                    User appUser = await userService.GetUserById(user.UserDesId);
                    Contact contact = await contactService.GetContactData(user.UserDesId);
                    UserInformationModel model = new UserInformationModel();
                    model.PhoneNumber = appUser.PhoneNumber;
                    model.UserName = contact.ContactName;
                    if (contact.Avatar != null)
                    {
                        model.Avatar = Convert.ToBase64String(contact.Avatar);
                    }
                    else
                    {
                        contact.Avatar = null;
                    }
                    if (appUser.Background != null)
                    {
                        model.Background = Convert.ToBase64String(appUser.Background);
                    }
                    else
                    {
                        model.Background = null;
                    }
                    model.Gender = appUser.Gender;
                    model.DateOfBirth = appUser.DateOfBirth;
                    model.Email = appUser.Email;
                    model.Id = appUser.Id;
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
