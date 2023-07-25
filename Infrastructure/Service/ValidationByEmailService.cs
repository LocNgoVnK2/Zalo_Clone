using Infrastructure.Entities;
using Infrastructure.Repository;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Infrastructure.Service
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ValidationType
    {
        ValidatedEmail = 0,
        ResetPassword = 1,
        ForgetPassword = 2
    }
    public enum ValidationRespond
    {
        Exception,
        IncorrectType,
        WrongCode,
        IsUsed,
        IsExpired,
        Success
    }
    public interface IValidationByEmailService
    {
        Task<string> CreateValidationCode(string email, ValidationType type);
        Task<ValidationRespond> ValidateCode(ValidationByEmail entity);
    }
    public class ValidationByEmailService : IValidationByEmailService
    {
        private IValidationByEmailRepository _repo;
        private IUtils utils;
        public ValidationByEmailService(IValidationByEmailRepository repo, IUtils utils)
        {
            this._repo = repo;
            this.utils = utils;
        }

        public async Task<string> CreateValidationCode(string email, ValidationType type)
        {
            var validationEntity = new ValidationByEmail
            {
                Email = email,
                ValidationType = (int)type,
                ExpiredTime = DateTime.Now.AddMinutes(5),
                ValidationCode = utils.GenerateRandomString(6)
            };
            bool result = await _repo.Add(validationEntity);
            if (result)
                return validationEntity.ValidationCode;
            return null;

        }

        public async Task<ValidationRespond> ValidateCode(ValidationByEmail entity)
        {
            var entites = _repo.GetAll();
            var result = await entites.FirstOrDefaultAsync(x => x.Email.Equals(entity.Email) && x.ValidationCode.Equals(entity.ValidationCode));
            if (result == null)
            {
                return ValidationRespond.WrongCode; //code không đúng
            }
            if (result.ValidationType != entity.ValidationType)
            {
                return ValidationRespond.IncorrectType;
            }
            if (result.IsActivated)
            {
                return ValidationRespond.IsUsed; //Code đã sử dụng
            }
            if (result.ExpiredTime < DateTime.Now)
            {
                return ValidationRespond.IsExpired; //Code hết hạn
            }
            result.IsActivated = true;
            bool canUpdate = await _repo.Update(result);
            if (canUpdate)
                return ValidationRespond.Success;
            return ValidationRespond.Exception;
        }
    }
}
