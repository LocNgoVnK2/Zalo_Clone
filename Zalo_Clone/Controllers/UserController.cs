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
        private readonly IEmailService emailService;
        private readonly IMapper mapper;
        private readonly IValidationByEmailService validationByEmailServices;
        private readonly IUtils utils;
        private readonly IUserContactService userContactService;
        private readonly IGroupUserService groupUserService;
        private readonly IMessageService messageService;
        private readonly IGroupChatService groupChatService;
        private readonly IContactService contactService;
        public UserController(IUserService userAccountService,
        IMapper mapper,
        IEmailService emailService,
        IValidationByEmailService validationByEmailServices,
        IUtils utils,
        IUserContactService userContactService,
        IGroupUserService groupUserService,
        IMessageService messageService,
        IGroupChatService groupChatService,
        IContactService contactService
        )
        {
            this.userAccountService = userAccountService;
            this.mapper = mapper;
            this.emailService = emailService;
            this.validationByEmailServices = validationByEmailServices;
            this.utils = utils;
            this.userContactService = userContactService;
            this.groupUserService = groupUserService;
            this.messageService = messageService;
            this.groupChatService = groupChatService;
            this.contactService = contactService;
        }
        [HttpGet("GetContactsOfUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContactsOfUser(string userID)
        {
            var userContactModels = new List<UserContactModel>();
            var contacts = await userContactService.GetContactsOfUserByTimeDesc(userID);
            foreach (var contact in contacts)
            {
                var lastMessage = (await messageService.GetMessagesOfUsersContact(userID, contact.ContactId))?.LastOrDefault();
                contact.LastMessage = lastMessage;

            }
            contacts = contacts.OrderByDescending(x => x.LastMessage?.SendTime).ToList();
            foreach (var contact in contacts)
            {
                var contactData = await contactService.GetContactData(contact.ContactId);
                var model = mapper.Map<UserContactModel>(contactData);
                model.LastMessageContent = contact.LastMessage?.Content;
                userContactModels.Add(model);
            }
            return Ok(userContactModels);

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
                ValidationCode = validationCode,
                ValidationType = (int)ValidationType.ResetPassword
            };
            string token = utils.ValidationByEmailEntityToToken(validationEntity);
            string subject = "Xin chào: " + user.UserName + " | Email reset password";
            string content = "Đây là email gửi tự động bởi hệ thống xác minh , vui lòng nhấn vào đường dẫn để reset password của bạn: " +
            "http://localhost:3000/renewPassword?token=" + token;
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
                    return NotFound("Validation code is used");
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
                            ValidationCode = validationCode,
                            ValidationType = (int)ValidationType.ValidatedEmail
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
        [HttpPost("ReSendToken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReSendToken(string token)
        {
            var entity = utils.TokenToValidationByEmailEntity(token);
            var emailExist = await userAccountService.GetSignUpUserByEmail(entity.Email);
            if (emailExist != null)
            {
                try
                {
                    var validationCode = await validationByEmailServices.CreateValidationCode(emailExist.Email, ValidationType.ValidatedEmail);
                    if (string.IsNullOrEmpty(validationCode))
                    {
                        return BadRequest("Can't create validation code");
                    }
                    var validationEntity = new ValidationByEmail()
                    {
                        Email = emailExist.Email,
                        ValidationCode = validationCode,
                        ValidationType = (int)ValidationType.ValidatedEmail
                    };
                    string newtoken = utils.ValidationByEmailEntityToToken(validationEntity);
                    string subject = "Xin chào: " + emailExist.Username + " | Email xác nhận lại ";
                    string content = "Đây là email gửi tự động bởi hệ thống xác minh , vui lòng nhấn vào đường dẫn để xác minh email của bạn: " +
                    "http://localhost:3000/validation?token=" + newtoken;
                    var message = new EmailMessage(emailExist.Email, subject, content);
                    emailService.SendEmail(message);
                    return Ok("User registered successfully, waiting for validation");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
            {
                return BadRequest("Email does not exist");
            }
        }

        [HttpPost("ReSendTokenResetPassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReSendTokenResetPassword(string token)
        {
            var entity = utils.TokenToValidationByEmailEntity(token);
            var emailExist = await userAccountService.GetUser(entity.Email);
            if (emailExist != null)
            {
                try
                {
                    var validationCode = await validationByEmailServices.CreateValidationCode(emailExist.Email, ValidationType.ResetPassword);
                    if (string.IsNullOrEmpty(validationCode))
                    {
                        return BadRequest("Can't create validation code");
                    }
                    var validationEntity = new ValidationByEmail()
                    {
                        Email = emailExist.Email,
                        ValidationCode = validationCode,
                        ValidationType = (int)ValidationType.ResetPassword
                    };
                    string newtoken = utils.ValidationByEmailEntityToToken(validationEntity);
                    string subject = "Xin chào: " + emailExist.UserName + " | Email xác nhận lại resest password";
                    string content = "Đây là email gửi tự động bởi hệ thống xác minh , vui lòng nhấn vào đường dẫn để reset password của bạn:" +
                    "http://localhost:3000/renewPassword?token=" + newtoken;
                    var message = new EmailMessage(emailExist.Email, subject, content);
                    emailService.SendEmail(message);
                    return Ok("User registered successfully, waiting for validation");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
            {
                return BadRequest("Email does not exist");
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
                    return NotFound("Validation code is used");
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
        [HttpGet("GetContactInformationById")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContactInformationById(string id)
        {
            try
            {
                var contactData = await contactService.GetContactData(id);
                var model = mapper.Map<ContactDataModel>(contactData);

                return Ok(model);

            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetUserInformation")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserInformation(string email)
        {
            try
            {
                var appUser = await userAccountService.GetUser(email);
                var userData = await contactService.GetContactData(appUser.Id);
                if (appUser != null && userData != null)
                {
                    UserInformationModel user = new();
                    user.Id = appUser.Id;
                    user.PhoneNumber = appUser.PhoneNumber;
                    user.UserName = appUser.UserName;
                    user.Email = appUser.Email;
                    user.Avatar = userData.Avatar;
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
        [HttpPost("UpdatePassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePassword(string token, string password)
        {
            var entity = utils.TokenToValidationByEmailEntity(token);
            User accountExist = await userAccountService.GetUser(entity.Email);
            if (accountExist != null)
            {
                try
                {
                    accountExist.Password = password;
                    bool result = await userAccountService.UpdatePassword(accountExist);
                    if (result)
                    {
                        return Ok(result);
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
            else
            {
                return BadRequest("Email does not exist");
            }
        }
    }
}
