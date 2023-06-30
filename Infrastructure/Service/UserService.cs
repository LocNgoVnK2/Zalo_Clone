using Infrastructure.Entities;
using Infrastructure.Repository;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
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
        Task<string> SignInAsync(User request);
        Task<bool> SignUpAsync(User request);
    }
    public class UserService : IUserService
    {
        private IUserRepository userAccountRepository;
   
        private IConfiguration configuration;

        public UserService(IUserRepository userAccountRepository,IConfiguration configuration)
        {
            this.userAccountRepository = userAccountRepository;
      
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


        public async Task<string> SignInAsync(User request)
        {
            var user = userAccountRepository.GetAll().Where(x=>x.Email.Equals(request.Email) && x.Password.Equals(request.Password)).FirstOrDefault();
             if (user == null)
             {
                 throw new Exception("Cannot find user");
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


        public async Task<bool> SignUpAsync(User request)
        {
            try
            {
                request.Id = GenerateRandomId();
                while (userAccountRepository.GetById(request.Id) == null)
                {
                    request.Id = GenerateRandomId();
                }
                var result = await userAccountRepository.Add(request);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to sign up.", ex);
            }
        }
        private string GenerateRandomId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var id = new string(Enumerable.Repeat(chars, 64)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return id;
        }

        public async Task<string> GetIdByEmailAsync(string email)
        {
            string userid = await userAccountRepository.GetAll().Where(x=>x.Email.Equals(email)).Select(s=>s.Id).FirstOrDefaultAsync();
            if(userid == null)
            {
                return null;
            }
            else
            {
                return userid;
            }
        }

    }
}
