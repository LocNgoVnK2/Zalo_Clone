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
            Task<List<Reaction>> reactions = _reactionService.GetAll().ToListAsync();
            List<Reaction> reactionList = await reactions;
            return mapper.Map < List < ReactionModel>>(reactionList);

        }
        [HttpGet("id")]
        [ProducesResponseType(typeof(ReactionModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByID(int id)
        {
            Task<Reaction> reaction = _reactionService.GetReaction(id);
            Reaction reactionAwait = await reaction;
            if (reactionAwait != null)
            {
                return Ok(mapper.Map<ReactionModel>(reactionAwait));
            }
            return NotFound();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ReactionModel reactionModel)
        {
            if(reactionModel.Id == 0)
            {
                return BadRequest();
            }
            Reaction reaction = mapper.Map<Reaction>(reactionModel);
            await _reactionService.AddReaction(reaction);
            return CreatedAtAction(nameof(GetByID), new {id = reactionModel.Id}, reactionModel);
        }
        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, ReactionModel reactionModel)
        {
            if(id == 0 || id != reactionModel.Id) {
                return BadRequest();
            }
            Reaction reaction = mapper.Map<Reaction>(reactionModel);
            await _reactionService.UpdateReaction(reaction);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            Reaction reaction = await _reactionService.GetReaction(id);
            if(id == 0 || reaction == null)
            {
                return BadRequest();
            }
            await _reactionService.RemoveReaction(reaction);
            return NoContent();
        } 
    }
}