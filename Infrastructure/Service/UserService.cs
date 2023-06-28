using Infrastructure.Entities;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    public interface IUserService
    {

        Task<User> GetUser(string email);
        void InsertUser(User userAccount);
        void UpdateUser(User userAccount);

        Task<string> GetIdByEmailAsync(string email);
        Task<string> SignInAsync(User request, string password);
        Task<IdentityResult> SignUpAsync(User request, string password);
    }
    public class UserService : IUserService
    {
        private IUserRepository userAccountRepository;
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private IConfiguration configuration;

        public UserService(IUserRepository userAccountRepository, UserManager<User> userManager, SignInManager<User> signInManager,IConfiguration configuration)
        {
            this.userAccountRepository = userAccountRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        public async Task<User> GetUser(string email)
        {
            return await userAccountRepository.GetAll().Where(X => X.Email.Equals(email)).FirstOrDefaultAsync();
        }
        public void InsertUser(User userAccount)
        {
            userAccountRepository.Add(userAccount);
        }
        public void UpdateUser(User userAccount)
        {
            userAccountRepository.Update(userAccount);
        }


        public async Task<string> SignInAsync(User request,string password)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
             if (user == null)
             {
                 throw new Exception("Cannot find user");
             }

             var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
             if (!result.Succeeded)
             {
                 return null;
             }

             var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Email, user.Email)
                // new Claim(ClaimTypes.Role, user.Type.ToString()) //Nghiên cứu phân quyền sau
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

             return token;
        }


        public async Task<IdentityResult> SignUpAsync(User request,string password)
        {

             var user = new User
              {
                  UserName = request.UserName,
                  NormalizedEmail = request.Email,
                  Email = request.Email,
                  IsActivated = false,
                  RestoreMail = request.RestoreMail,
                  PhoneNumber = request.PhoneNumber
              };


              var result = await userManager.CreateAsync(user,password);

              return result;
        }

        public async Task<string> GetIdByEmailAsync(string email)
        {
            User user = await userManager.FindByEmailAsync(email);
             return user.Id;
        }
    }
}
