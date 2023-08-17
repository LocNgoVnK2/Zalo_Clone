using Infrastructure.Entities;
using Infrastructure.Repository;
using Infrastructure.Utils;
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
        Task<User> GetUserById(string id);
        Task<bool> InsertUser(User userAccount);

        Task<bool> UpdateUser(User userAccount);

        Task<string> GetIdByEmailAsync(string email);
        Task<string> SignInAsync(User request);
        Task<bool> SignUpAsync(SignUpUser request);
        Task<bool> verifyEmailAsync(string email);

        Task<SignUpUser> GetSignUpUserByEmail(string email);
        Task<bool> CompleteSignUp(string email);
        Task<bool> UpdatePassword(User request);
        Task<List<User>> GetRecommandUsers(string id);
    }
    public class UserService : IUserService
    {
        private IUserRepository userAccountRepository;
        private readonly IContactRepository contactRepository;
        private IConfiguration configuration;
        private IUtils utils;
        private ISignUpUserRepository signUpUserRepository;
        private IFriendListRepository friendListRepository;
        private IFriendRequestRepository friendRequestRepository;

        public UserService(IUserRepository userAccountRepository,
        IConfiguration configuration,
        IUtils utils,
        ISignUpUserRepository signUpUserRepository,
        IContactRepository contactRepository,
        IFriendListRepository friendListRepository,
        IFriendRequestRepository friendRequestRepository
        )
        {
            this.userAccountRepository = userAccountRepository;
            this.contactRepository = contactRepository;
            this.configuration = configuration;
            this.utils = utils;
            this.signUpUserRepository = signUpUserRepository;
            this.friendListRepository = friendListRepository;
            this.friendRequestRepository = friendRequestRepository;
        }

        public async Task<User> GetUser(string email)
        {
            return await userAccountRepository.GetAll().FirstOrDefaultAsync(X => X.Email.Equals(email))!;
        }
        public async Task<bool> InsertUser(User userAccount)
        {
            bool result = await contactRepository.Add(new Contact()
            {
                Id = userAccount.Id!,
                ContactName = userAccount.UserName
            });
            result = await userAccountRepository.Add(userAccount);
            return result;
        }

        public async Task<bool> UpdateUser(User userAccount)
        {

            var contact = await contactRepository.GetById(userAccount.Id);
            contact.ContactName = userAccount.UserName;
            bool result = await contactRepository.Update(contact);
            result = await userAccountRepository.Update(userAccount);
            return result;

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

        public async Task<bool> SignUpAsync(SignUpUser request)
        {
            try
            {
                MD5 md5 = MD5.Create();
                var bytesHash = md5.ComputeHash(Encoding.UTF8.GetBytes(request.Password!));
                var passHash = BitConverter.ToString(bytesHash).Replace("-", "");
                request.Password = passHash;
                var result = await signUpUserRepository.Add(request);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to sign up.", ex);
            }
        }
        public async Task<bool> UpdatePassword(User request)
        {
            try
            {
                MD5 md5 = MD5.Create();
                var bytesHash = md5.ComputeHash(Encoding.UTF8.GetBytes(request.Password!));
                var passHash = BitConverter.ToString(bytesHash).Replace("-", "");
                request.Password = passHash;
                bool result = await userAccountRepository.Update(request);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to sign up.", ex);
            }
        }

        public async Task<string> GetIdByEmailAsync(string email)
        {
            string userid = await userAccountRepository.GetAll().Where(x => x.Email.Equals(email)).Select(s => s.Id).FirstOrDefaultAsync()!;
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

        public async Task<SignUpUser> GetSignUpUserByEmail(string email)
        {
            return await signUpUserRepository.GetById(email);
        }

        public async Task<bool> CompleteSignUp(string email)
        {
            var user = await GetSignUpUserByEmail(email);
            if (user == null)
                return false;
            var appUser = new User()
            {
                Email = user.Email,
                UserName = user.Username,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                DateOfBirth = (DateTime)user.DateOfBirth!
            };
            do
            {
                appUser.Id = utils.GenerateRandomString(64);
            } while (await userAccountRepository.GetById(appUser.Id) != null);
            bool result = await contactRepository.Add(new Contact()
            {
                Id = appUser.Id,
                ContactName = user.Username
            });
            result = await userAccountRepository.Add(appUser);
            result = await signUpUserRepository.Delete(user);
            return result;
        }

        public async Task<User> GetUserById(string id)
        {
            return await userAccountRepository.GetById(id);
        }
        public async Task<List<User>> GetRecommandUsers(string id)
        {
            var listUsers = userAccountRepository.GetAll();
            User userSrc = await userAccountRepository.GetById(id);
            var friends = await friendListRepository.GetAll().Where(f => f.User1.Equals(id) || f.User2.Equals(id)).ToListAsync();
            var friendRequestsToSend =  await friendRequestRepository.GetAll().Where(o => o.User1.Equals(id)).ToListAsync();
            var friendRequestsToReceiver =  await friendRequestRepository.GetAll().Where(o => o.User2.Equals(id)).ToListAsync();
           
            var nonFriends = listUsers.ToList().Where(user =>
                        !friends.Any(friendId => friendId.User2 == user.Id || friendId.User1 == user.Id)
                        && !friendRequestsToSend.Any(request => request.User2 == user.Id)
                        && !friendRequestsToReceiver.Any(request => request.User1 == user.Id)
                        && user.Id != id
                        ).ToList();
            var recommandUsers = new List<User>();
            Random random = new Random();
            while (recommandUsers.Count < 5 && nonFriends.Count > 0)
            {
                var randomIndex = random.Next(nonFriends.Count);
                var randomUser = nonFriends[randomIndex];
                recommandUsers.Add(randomUser);
                nonFriends.RemoveAt(randomIndex);
            }
            return recommandUsers;
        }
    }
}
