using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapTheMuseApi.Models;
using Microsoft.AspNetCore.Authorization;
using MapTheMuseApi.Dtos;

namespace MapTheMuseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var list = await _roleManager.Roles
                            .Select(r => new RoleDto
                            {
                                Id = r.Id,
                                Name = r.Name
                            })
                            .ToListAsync();

            return Ok(list);
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }
            var dto = new RoleDto
            {
                Id = roleId,
                Name = role.Name
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleNameDto dto)
        {
            var role = new IdentityRole(dto.RoleName);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return CreatedAtAction(
                    nameof(GetRole),
                    new { roleId = role.Id },
                    new RoleDto { Id = role.Id, Name = role.Name! }
                  );
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId, [FromBody] UpdateRoleDto dto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            role.Name = dto.NewRoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Ok("Role updated successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok("Role deleted successfully.");
            }

            return BadRequest(result.Errors);
        }

    }
}
