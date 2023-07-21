using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zalo_Clone.Models;

namespace Zalo_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private readonly IValidationByEmailService validationByEmailService;
        private readonly IMapper mapper;
        private readonly IUtils utils;
        private readonly IEmailService emailService;
        public ValidationController(IValidationByEmailService validationByEmailService,
         IMapper mapper,
         IUtils utils,
         IEmailService emailService)
        {
            this.validationByEmailService = validationByEmailService;
            this.mapper = mapper;
            this.utils = utils;
            this.emailService = emailService;
        }
        [HttpPost("CreateCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCode(string email, ValidationType type)
        {
            string code = await validationByEmailService.CreateValidationCode(email, type);
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Không thể tạo validation code");

            }
            string subject = "Xin chào: "+ email;
            string content = "Đây là email gửi tự động bởi hệ thống xác minh , vui lòng nhấn vào đường dẫn để xác minh email của bạn: " +
            "http://localhost:3000/validation?token="+utils.EncodeInformation(email,code,type.ToString());
            var message = new EmailMessage(email, subject, content);
            emailService.SendEmail(message);
            return Ok(code);
        }
        [HttpPost("ValidateCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateCode(ValidationByEmailModel validationModel)
        {
            var entity = mapper.Map<ValidationByEmail>(validationModel);
            entity.ValidationType = (int)validationModel.ValidationType;
            ValidationRespond result = await validationByEmailService.ValidateCode(entity);
            switch (result)
            {
                case ValidationRespond.Success:
                    return Ok();

                case ValidationRespond.IncorrectType:
                    return BadRequest("Code không trùng loại");
                case ValidationRespond.IsExpired:
                    return BadRequest("Code hết hạn");
                case ValidationRespond.IsUsed:
                    return BadRequest("Code đã được sử dụng");
                case ValidationRespond.WrongCode:
                    return BadRequest("Code không đúng");
                default:
                    return BadRequest("Lỗi không xác định");
            }
        }
        [HttpPost("Encode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Encode(params string[] parameters)
        {
            var result = utils.EncodeInformation(parameters);
            return Ok(result);
        }
        [HttpPost("Decode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Decode(string code)
        {
            var result = utils.DecodeInformation(code);
            return Ok(result);
        }
    }
}
