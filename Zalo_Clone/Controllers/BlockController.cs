using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zalo_Clone.Models;

namespace Zalo_Clone.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class BlockController : ControllerBase
    {
        private readonly IBlockService blockService;
        private readonly IMapper mapper;
        public BlockController(IBlockService blockService, IMapper mapper)
        {
            this.blockService = blockService;
            this.mapper = mapper;   
        }
        [HttpPost]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Block(BlockModel blockModel)
        {
            bool result = await blockService.Block(blockModel.UserSrcId, blockModel.UserDesId);
            if(result)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpGet(Name = "GetBlockList")]
        public async Task<List<BlockList>> GetBlockListOfUser(string userID)
        {
            List<BlockList> blockList = await blockService.GetBlockListOfUser(userID);
            return blockList;

        }

        [HttpPost("Unblock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Unblock(BlockModel blockModel)
        {
            bool result = await blockService.UnBlock(blockModel.UserSrcId, blockModel.UserDesId);
            if (result)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}