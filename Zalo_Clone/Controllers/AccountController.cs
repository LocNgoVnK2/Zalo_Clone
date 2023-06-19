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
    public class AccountController : ControllerBase
    {
        private readonly IUserAccountService userAccountService;
        private readonly IUserDataService userDataService;
        private readonly IMapper mapper;
        public AccountController(IUserAccountService userAccountService, IMapper mapper, IUserDataService userDataService)
        {
            this.userAccountService = userAccountService;
            this.mapper = mapper;
            this.userDataService = userDataService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel account)
        {
            UserAccount request = mapper.Map<UserAccount>(account);
            try
            {
                var result = await userAccountService.SignUpAsync(request);
                if (result.Succeeded)
                {
                    string uid = await userAccountService.GetIdByEmailAsync(request.Email);
                    UserData uData = new UserData()
                    {
                        Id = uid
                    };
                    userDataService.AddUserData(uData);
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

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInModel account)
        {
            UserAccount request = mapper.Map<UserAccount>(account);
            try
            {
                var token = await userAccountService.SignInAsync(request);
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
    }
}
