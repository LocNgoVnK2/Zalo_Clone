using Infrastructure.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{

    public interface IUserAccountService
    {

        Task<UserAccount> GetUserAccount(string email);
        void InsertUserAccount(UserAccount userAccount);
        void UpdateUserAccount(UserAccount userAccount);

        Task<string> GetIdByEmailAsync(string email);
        Task<string> SignInAsync(UserAccount request);
        Task<IdentityResult> SignUpAsync(UserAccount request);
    }
    public class UserAccountService : IUserAccountService
    {
        private IUserAccountRepository userAccountRepository;
/*        private UserManager<UserAccount> userManager;
        private SignInManager<UserAccount> signInManager;*/
        private IConfiguration configuration;

    /*    public UserAccountService(IUserAccountRepository userAccountRepository, UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager,IConfiguration configuration)
        {
            this.userAccountRepository = userAccountRepository;
*//*            this.userManager = userManager;
            this.signInManager = signInManager;*//*
            this.configuration = configuration;
        }*/

        public async Task<UserAccount> GetUserAccount(string email)
        {
            return await userAccountRepository.GetById(email);
        }
        public void InsertUserAccount(UserAccount userAccount)
        {
            userAccountRepository.Add(userAccount);
        }
        public void UpdateUserAccount(UserAccount userAccount)
        {
            userAccountRepository.Update(userAccount);
        }


        public async Task<string> SignInAsync(UserAccount request)
        {
            /*    var user = await userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    throw new Exception("Cannot find user");
                }

                var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    return null;
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Type.ToString())
                };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: credentials
                );

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return token;*/
            return null;
        }


        public async Task<IdentityResult> SignUpAsync(UserAccount request)
        {

            /*  var user = new UserAccount
              {
                  UserName = request.UserName,
                  NormalizedEmail = request.Email,
                  Email = request.Email,
                  Type = request.Type,
                  IsConfirmed = false,
                  IsActivated = false,
                  RestoreMail = request.RestoreMail,
                  Password = request.Password,
                  PhoneNumber = request.PhoneNumber
              };


              var result = await userManager.CreateAsync(user, request.Password);

              return result;*/
            return null;
        }

        public async Task<string> GetIdByEmailAsync(string email)
        {
            /* UserAccount user = await userManager.FindByEmailAsync(email);
             return user.Id;*/
            return null;
        }
    }
}
