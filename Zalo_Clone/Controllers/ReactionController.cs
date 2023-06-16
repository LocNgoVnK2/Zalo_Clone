using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zalo_Clone.Models;

namespace Zalo_Clone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;
        private readonly IMapper mapper;
        public ReactionController(IReactionService reactionService,IMapper mapper)
        {
            this._reactionService = reactionService;
            this.mapper = mapper;   
        }
        [HttpGet(Name = "GetReactions")]
        public async Task<List<ReactionModel>> GetAll()
        {
            return await mapper.Map<IQueryable<ReactionModel>>(_reactionService.GetAll()).ToListAsync();
        }
        [HttpGet("id")]
        [ProducesResponseType(typeof(ReactionModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByID(int id)
        {
            Reaction reaction = _reactionService.GetReaction(id)
            if (reaction != null)
            {
                return Ok(mapper.Map<ReactionModel>(reaction));
            }
            return NotFound();
        }
        [HttpPost]
        public void 
    }
}