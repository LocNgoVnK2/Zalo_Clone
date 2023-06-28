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
    public class UserController : ControllerBase
    {
        private readonly IUserService userAccountService;
        private readonly IUserDataService userDataService;
        private readonly IMapper mapper;
        public UserController(IUserService userAccountService, IMapper mapper, IUserDataService userDataService)
        {
            this.userAccountService = userAccountService;
            this.mapper = mapper;
            this.userDataService = userDataService;
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
                    var result = await userAccountService.SignUpAsync(request, model.Password);
                    if (result.Succeeded)
                    {
                        string uid = await userAccountService.GetIdByEmailAsync(request.Email);
                        UserData uData = new UserData()
                        {
                            Id = uid,
                            Gender = model.Gender,
                            DateOfBirth = model.DateOfBirth

                        };
                        await userDataService.AddUserData(uData);
                        return Ok("User registered successfully");
                    }
                    else
                    {
                        return BadRequest(result.Errors);
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
                var token = await userAccountService.SignInAsync(request, account.Password);
                if (token != null)
                {
                    return Ok(new { Token = token });
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

        [HttpGet("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            
            try
            {
                var user = await userAccountService.GetUser(email);
                if (user != null)
                {
                    return Ok(user.Id);
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

    }
}
