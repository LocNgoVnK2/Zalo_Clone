using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Zalo_Clone.Models;

namespace Zalo_Clone.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReactionController : ControllerBase
    {
        private readonly ReactionService _reactionService;
        private readonly IMapper mapper;
        public ReactionController(ReactionService reactionService,IMapper mapper)
        {
            this._reactionService = reactionService;
            this.mapper = mapper;   
        }
        [HttpGet(Name = "GetReactions")]
        public  IEnumerable<ReactionModel> GetAll()
        {
            return mapper.Map<List<ReactionModel>>(_reactionService.GetAll());
        }
    }
}