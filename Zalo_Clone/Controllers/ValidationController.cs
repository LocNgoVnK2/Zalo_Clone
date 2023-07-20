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
    public class ValidationController : ControllerBase
    {
        private readonly IValidationByEmailService validationByEmailService;
        private readonly IMapper mapper;
        public ValidationController(IValidationByEmailService validationByEmailService, IMapper mapper)
        {
            this.validationByEmailService = validationByEmailService;
            this.mapper = mapper;

        }
        [HttpPost("CreateCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCode(string email, ValidationType type)
        {
            bool result = await validationByEmailService.CreateValidationCode(email, type);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
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

    }
}
