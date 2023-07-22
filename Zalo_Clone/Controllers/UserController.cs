using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Runtime.CompilerServices;
using Zalo_Clone.Models;

namespace Zalo_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userAccountService;
        private readonly IUserDataService userDataService;
        private readonly IEmailService emailService;
        private readonly IMapper mapper;
        private readonly IValidationByEmailService validationByEmailServices;
        public UserController(IUserService userAccountService, IMapper mapper, IUserDataService userDataService, IEmailService emailService, IValidationByEmailService validationByEmailServices)
        {
            this.userAccountService = userAccountService;
            this.mapper = mapper;
            this.userDataService = userDataService;
            this.emailService = emailService;
            this.validationByEmailServices = validationByEmailServices;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var request = mapper.Map<User>(model);
            var checkEmailExist = await userAccountService.GetIdByEmailAsync(request.Email);
            if (checkEmailExist != null)
            {
                return BadRequest("Email exist");
            }
            else
            {
                try
                {
                    bool result = await userAccountService.SignUpAsync(request);
                    if (result)
                    {
                        string uid = await userAccountService.GetIdByEmailAsync(request.Email);
                        UserData uData = new UserData()
                        {
                            Id = uid,
                            Gender = model.Gender,
                            DateOfBirth = model.DateOfBirth

                        };
                        result = await userDataService.AddUserData(uData);
                        result = await validationByEmailServices.CreateValidationCode(request.Email, ValidationType.ValidatedEmail);
                        return Ok("User registered successfully");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInModel account)
        {
            var request = mapper.Map<User>(account);
            try
            {
                var token = await userAccountService.SignInAsync(request);
                if (token != null)
                {
                    return Ok(new { Token = token });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            
            try
            {
                var user = await userAccountService.GetUser(email);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest("Invalid email or password");
                }
            }
            catch (Exception ex){
                return StatusCode(500, ex.Message);
            }
        }
    
      
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(string[] emailAddresses) {
            string validationcode=null;
            User user = null;
            foreach (var emailAddress in emailAddresses)
            {
                 user = await userAccountService.GetUser(emailAddress);
             
            }
            string subject = "Xin chào :"+ user.UserName;
            string content = "Đây là email gửi tự động bởi hệ thống xác minh https://github.com/LocNgoVnK2/Zalo_Clone , Mã xác minh của bạn là : " + validationcode;
            var message = new EmailMessage(emailAddresses, subject, content);
            emailService.SendEmail(message);
            return Ok();
        
        }
        /*
        [HttpPost("EnterValidationCode")]
        public async Task<IActionResult> EnterValidationCode(string emailAddresses,string validationCode)
        {

            User user = await userAccountService.GetUser(emailAddresses);
            if(user == null)
            {
                return BadRequest();
            }
            if(user.ValidationCode == validationCode)
            {
                bool result = await userAccountService.verifyEmailAsync(user.Email);
                if (result)
                {
                    return Ok("Verify Email success");
                }
                else
                {
                    return StatusCode(500, "Failed to verify Email");
                }
            }
            return Ok();

        }
        */
    }
}
