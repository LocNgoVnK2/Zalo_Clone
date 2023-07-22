using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Infrastructure.Utils;
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
        private readonly IUtils utils;
        public UserController(IUserService userAccountService,
         IMapper mapper, IUserDataService userDataService,
          IEmailService emailService,
           IValidationByEmailService validationByEmailServices,
           IUtils utils
            )
        {
            this.userAccountService = userAccountService;
            this.mapper = mapper;
            this.userDataService = userDataService;
            this.emailService = emailService;
            this.validationByEmailServices = validationByEmailServices;
            this.utils = utils;
        }

        [HttpPost("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await userAccountService.GetUser(email);
            if (user == null)
                return BadRequest("Couldn't find user of this email address");
            var validationCode = await validationByEmailServices.CreateValidationCode(email, ValidationType.ResetPassword);
            if (string.IsNullOrEmpty(validationCode))
            {
                return StatusCode(500, "Can't create validation code");
            }
            var validationEntity = new ValidationByEmail()
            {
                Email = email,
                ValidationCode = validationCode
            };
            string token = utils.ValidationByEmailEntityToToken(validationEntity);
            string subject = "Xin chào: " + user.UserName;
            string content = "Đây là email gửi tự động bởi hệ thống xác minh , vui lòng nhấn vào đường dẫn để reset password của bạn: " +
            "http://localhost:3000/validation?token=" + token;
            var message = new EmailMessage(email, subject, content);
            emailService.SendEmail(message);
            return Ok("Sent email successfully, waiting for validation");
        }
          [HttpPost("ValidateResetPassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidateResetPassword(string token)
        {
            var entity = utils.TokenToValidationByEmailEntity(token);
            entity.ValidationType = (int)ValidationType.ResetPassword;
            ValidationRespond respond = await validationByEmailServices.ValidateCode(entity);
            switch (respond)
            {
                case ValidationRespond.Success:
                    //do something
                  
                        return Ok("Validate email successfully");
             

                case ValidationRespond.IncorrectType:
                    return BadRequest("Incorrect validation type");
                case ValidationRespond.IsExpired:
                    return BadRequest("Validation code is expired");
                case ValidationRespond.IsUsed:
                    return BadRequest("Validation code is used");
                case ValidationRespond.WrongCode:
                    return BadRequest("Wrong validation code");
                default:
                    return BadRequest("Unexpected error");
            }
        }
        [HttpPost("SignUp")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var request = mapper.Map<SignUpUser>(model);
            var checkEmailExist = await userAccountService.GetIdByEmailAsync(request.Email);
            if (checkEmailExist != null)
            {
                return BadRequest("Email exist");
            }
            var signUpUser = await userAccountService.GetSignUpUserByEmail(request.Email);
            if (signUpUser != null)
            {
                return BadRequest("This email is currently using for sign up");
            }
            else
            {
                try
                {
                    bool result = await userAccountService.SignUpAsync(request);
                    if (result)
                    {
                        var validationCode = await validationByEmailServices.CreateValidationCode(request.Email, ValidationType.ValidatedEmail);
                        if (string.IsNullOrEmpty(validationCode))
                        {
                            return BadRequest("Can't create validation code");

                        }
                        var validationEntity = new ValidationByEmail()
                        {
                            Email = request.Email,
                            ValidationCode = validationCode
                        };
                        string token = utils.ValidationByEmailEntityToToken(validationEntity);
                        string subject = "Xin chào: " + request.Username;
                        string content = "Đây là email gửi tự động bởi hệ thống xác minh , vui lòng nhấn vào đường dẫn để xác minh email của bạn: " +
                        "http://localhost:3000/validation?token=" + token;
                        var message = new EmailMessage(request.Email, subject, content);
                        emailService.SendEmail(message);
                        return Ok("User registered successfully, waiting for validation");
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
        [HttpPost("ValidateSignUp")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidateSignUp(string token)
        {
            var entity = utils.TokenToValidationByEmailEntity(token);
            entity.ValidationType = (int)ValidationType.ValidatedEmail;
            ValidationRespond respond = await validationByEmailServices.ValidateCode(entity);
            switch (respond)
            {
                case ValidationRespond.Success:
                    bool result = await userAccountService.CompleteSignUp(entity.Email);
                    if (result)
                        return Ok("Validate email successfully");
                    return BadRequest("Unexpected error");

                case ValidationRespond.IncorrectType:
                    return BadRequest("Incorrect validation type");
                case ValidationRespond.IsExpired:
                    return BadRequest("Validation code is expired");
                case ValidationRespond.IsUsed:
                    return BadRequest("Validation code is used");
                case ValidationRespond.WrongCode:
                    return BadRequest("Wrong validation code");
                default:
                    return BadRequest("Unexpected error");
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

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("verifyEmail")]
        public async Task<IActionResult> verifyEmail(string email)
        {
            try
            {
                bool result = await userAccountService.verifyEmailAsync(email);
                if (result == true)
                {
                    return Ok("verify Email success");
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
    
      
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(string[] emailAddresses)
        {
            string validationcode = null;
            User user = null;
            foreach (var emailAddress in emailAddresses)
            {

                user = await userAccountService.GetUser(emailAddress);
                //   validationcode = user.ValidationCode;
            }
            string subject = "Xin chào :" + user.UserName;
            string content = "Đây là email gửi tự động bởi hệ thống xác minh , Mã xác minh của bạn là : " + validationcode;

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
