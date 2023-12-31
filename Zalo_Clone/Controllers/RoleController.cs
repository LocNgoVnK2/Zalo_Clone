﻿using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zalo_Clone.ModelViews;

namespace Zalo_Clone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService,IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetRole(string name)
        {
            var role = _roleService.GetRole(name);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleService.GetAllRoles().ToList();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string name)
        {
            var result = await _roleService.AddRole(name);
            if (!result)
                return BadRequest("Failed to add role.");

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole(RoleModel model)
        {
            Role role = _mapper.Map<Role>(model);
            var result = await _roleService.UpdateRole(role);
            if (!result)
                return BadRequest("Failed to update role.");

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveRole(RoleModel model)
        {
            Role role = _mapper.Map<Role>(model);
            var result = await _roleService.RemoveRole(role);
            if (!result)
                return BadRequest("Failed to remove role.");

            return Ok();
        }
    }
}
