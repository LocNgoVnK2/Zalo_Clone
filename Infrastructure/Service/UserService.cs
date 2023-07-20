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
using System.Security.Cryptography;
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
        Task<bool> verifyEmailAsync(string email);
    }
    public class UserService : IUserService
    {
        private IUserRepository userAccountRepository;

        private IConfiguration configuration;

        public UserService(IUserRepository userAccountRepository, IConfiguration configuration)
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

            var user = userAccountRepository.GetAll().Where(x => x.Email.Equals(request.Email)).FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            var md5 = MD5.Create();
            var bytesHash = md5.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
            var passHash = BitConverter.ToString(bytesHash).Replace("-", "");
            if (user.Password == passHash)
            {
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
            else
            {
                return null;
            }
        }

        public async Task<bool> SignUpAsync(User request)
        {
            try
            {
                do
                {
                    request.Id = GenerateRandomId();
                } while (await userAccountRepository.GetById(request.Id) != null);
                MD5 md5 = MD5.Create();

                var bytesHash = md5.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
                var passHash = BitConverter.ToString(bytesHash).Replace("-", "");
                request.Password = passHash;

                request.ValidationCode = GenerateRandomValidationCode();

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
        private string GenerateRandomValidationCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var validationCode = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return validationCode;
        }
        public async Task<string> GetIdByEmailAsync(string email)
        {
            string userid = await userAccountRepository.GetAll().Where(x => x.Email.Equals(email)).Select(s => s.Id).FirstOrDefaultAsync();
            if (userid == null)
            {
                return null;
            }
            else
            {
                return userid;
            }
        }
        public async Task<bool> verifyEmailAsync(string email)
        {
            User user = await userAccountRepository.GetAll().Where(x => x.Email.Equals(email)).FirstOrDefaultAsync();
            if (user != null)
            {
                user.EmailConfirmed = true;
            }
            try
            {
                bool result = await userAccountRepository.Update(user);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to sign up.", e);
            }
        }
    }
}
